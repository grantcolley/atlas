using Atlas.Core.Constants;
using Atlas.Core.Exceptions;
using Atlas.Core.Models;
using Atlas.Data.Access.EF.Context;
using Atlas.Data.Access.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Atlas.Data.Access.EF.Data
{
    public class AdministrationData(ApplicationDbContext applicationDbContext, ILogger<AdministrationData> logger) 
        : AuthorisationData<AdministrationData>(applicationDbContext, logger), IAdministrationData
    {
        public async Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _applicationDbContext.Users
                    .AsNoTracking()
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex);
            }
        }

        public async Task<User> GetUserAsync(int userId, CancellationToken cancellationToken)
        {
            try
            {
                User user = await _applicationDbContext.Users
                        .Include(u => u.Roles)
                        .AsNoTracking()
                        .FirstAsync(u => u.UserId.Equals(userId), cancellationToken)
                        .ConfigureAwait(false);

                user.RoleChecklist = await GetRoleChecklistAsync(user.Roles, cancellationToken)
                    .ConfigureAwait(false);

                if (Authorisation == null
                    || !Authorisation.HasPermission(Auth.ADMIN_WRITE))
                {
                    user.IsReadOnly = true;
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex, $"UserId={userId}");
            }
        }

        public async Task<User> CreateUserAsync(User addUser, CancellationToken cancellationToken)
        {
            using IDbContextTransaction transaction = _applicationDbContext.Database.BeginTransaction();

            try
            {
                User user = new()
                {
                    Name = addUser.Name,
                    Email = addUser.Email
                };

                await _applicationDbContext.Users
                    .AddAsync(user, cancellationToken)
                    .ConfigureAwait(false);

                await _applicationDbContext
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                List<Role> roles = ExtractSelectedRoles(addUser.RoleChecklist);

                if (roles.Count > 0)
                {
                    user.Roles.AddRange(roles);

                    await _applicationDbContext
                        .SaveChangesAsync(cancellationToken)
                        .ConfigureAwait(false);
                }

                user.RoleChecklist = await GetRoleChecklistAsync(user.Roles, cancellationToken)
                    .ConfigureAwait(false);

                if (Authorisation == null
                    || !Authorisation.HasPermission(Auth.ADMIN_WRITE))
                {
                    user.IsReadOnly = true;
                }

                await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);

                return user;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);

                throw new AtlasException(ex.Message, ex);
            }
        }

        public async Task<User> UpdateUserAsync(User updateUser, CancellationToken cancellationToken)
        {
            try
            {
                User user = await _applicationDbContext.Users
                    .Include(u => u.Roles)
                    .FirstAsync(u => u.UserId.Equals(updateUser.UserId), cancellationToken)
                    .ConfigureAwait(false);

                _applicationDbContext
                    .Entry(user)
                    .CurrentValues.SetValues(updateUser);

                List<Role> roles = ExtractSelectedRoles(updateUser.RoleChecklist);

                List<Role> removeRoles = user.Roles
                    .Where(ur => !roles.Any(r => r.RoleId.Equals(ur.RoleId)))
                    .ToList();

                foreach (Role role in removeRoles)
                {
                    user.Roles.Remove(role);
                }

                List<Role> addRoles = roles
                    .Where(ur => !user.Roles.Any(r => r.RoleId.Equals(ur.RoleId)))
                    .ToList();

                user.Roles.AddRange(addRoles);

                _applicationDbContext.Users.Update(user);

                await _applicationDbContext
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                user.RoleChecklist = await GetRoleChecklistAsync(user.Roles, cancellationToken)
                    .ConfigureAwait(false);

                if (Authorisation == null
                    || !Authorisation.HasPermission(Auth.ADMIN_WRITE))
                {
                    user.IsReadOnly = true;
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex, $"UserId={updateUser.UserId}");
            }
        }

        public async Task<int> DeleteUserAsync(int userId, CancellationToken cancellationToken)
        {
            try
            {
                User user = await _applicationDbContext.Users
                    .FirstAsync(u => u.UserId.Equals(userId), cancellationToken)
                    .ConfigureAwait(false);

                _applicationDbContext.Users.Remove(user);

                return await _applicationDbContext
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex, $"UserId={userId}");
            }
        }

        public async Task<IEnumerable<Permission>> GetPermissionsAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _applicationDbContext.Permissions
                    .AsNoTracking()
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex);
            }
        }

        public async Task<Permission> GetPermissionAsync(int permissionId, CancellationToken cancellationToken)
        {
            try
            {
                Permission permission = await _applicationDbContext.Permissions
                    .AsNoTracking()
                    .Include(p => p.Roles)
                    .FirstAsync(p => p.PermissionId.Equals(permissionId), cancellationToken)
                    .ConfigureAwait(false);

                if (Authorisation == null
                    || !Authorisation.HasPermission(Auth.ADMIN_WRITE))
                {
                    permission.IsReadOnly = true;
                }

                return permission;
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex, $"PermissionId={permissionId}");
            }
        }

        public async Task<Permission> CreatePermissionAsync(Permission permission, CancellationToken cancellationToken)
        {
            try
            {
                await _applicationDbContext.Permissions
                    .AddAsync(permission, cancellationToken)
                    .ConfigureAwait(false);

                await _applicationDbContext
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                if (Authorisation == null
                    || !Authorisation.HasPermission(Auth.ADMIN_WRITE))
                {
                    permission.IsReadOnly = true;
                }

                return permission;
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex);
            }
        }

        public async Task<Permission> UpdatePermissionAsync(Permission permission, CancellationToken cancellationToken)
        {
            try
            {
                Permission existing = await _applicationDbContext.Permissions
                    .FirstOrDefaultAsync(p => p.PermissionId.Equals(permission.PermissionId), cancellationToken)
                    .ConfigureAwait(false)
                    ?? throw new NullReferenceException(
                        $"{nameof(permission)} PermissionId {permission.PermissionId} not found.");

                _applicationDbContext
                    .Entry(existing)
                    .CurrentValues.SetValues(permission);

                await _applicationDbContext
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                if (Authorisation == null
                    || !Authorisation.HasPermission(Auth.ADMIN_WRITE))
                {
                    permission.IsReadOnly = true;
                }

                return permission;
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex, $"PermissionId={permission.PermissionId}");
            }
        }

        public async Task<int> DeletePermissionAsync(int permissionId, CancellationToken cancellationToken)
        {
            try
            {
                Permission permission = await _applicationDbContext.Permissions
                    .FirstAsync(p => p.PermissionId.Equals(permissionId), cancellationToken)
                    .ConfigureAwait(false);

                _applicationDbContext.Permissions.Remove(permission);

                return await _applicationDbContext
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex, $"PermissionId={permissionId}");
            }
        }

        public async Task<IEnumerable<Role>> GetRolesAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _applicationDbContext.Roles
                    .Include(p => p.Permissions)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex);
            }
        }

        public async Task<Role> GetRoleAsync(int roleId, CancellationToken cancellationToken)
        {
            try
            {
                Role role = await _applicationDbContext.Roles
                    .Include(r => r.Users)
                    .Include(r => r.Permissions)
                    .AsNoTracking()
                    .FirstAsync(r => r.RoleId.Equals(roleId), cancellationToken)
                    .ConfigureAwait(false);

                role.PermissionChecklist = await GetPermissionChecklistAsync(role.Permissions, cancellationToken)
                    .ConfigureAwait(false);

                if (Authorisation == null
                    || !Authorisation.HasPermission(Auth.ADMIN_WRITE))
                {
                    role.IsReadOnly = true;
                }

                return role;
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex, $"RoleId={roleId}");
            }
        }

        public async Task<Role> CreateRoleAsync(Role addRole, CancellationToken cancellationToken)
        {
            using IDbContextTransaction transaction = _applicationDbContext.Database.BeginTransaction();

            try
            {
                Role role = new()
                {
                    Name = addRole.Name,
                    Description = addRole.Description
                };

                await _applicationDbContext.Roles
                    .AddAsync(role, cancellationToken)
                    .ConfigureAwait(false);

                await _applicationDbContext
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                List<int> permissionIds = ExtractSelectedPemissions(addRole.PermissionChecklist);

                if (permissionIds.Count > 0)
                {
                    IEnumerable<Permission> permissions = await _applicationDbContext.Permissions
                        .AsNoTracking()
                        .Where(p => permissionIds.Contains(p.PermissionId))
                        .ToListAsync(cancellationToken);

                    role.Permissions.AddRange(permissions);

                    await _applicationDbContext
                        .SaveChangesAsync(cancellationToken)
                        .ConfigureAwait(false);
                }

                role.PermissionChecklist = await GetPermissionChecklistAsync(role.Permissions, cancellationToken)
                    .ConfigureAwait(false);

                if (Authorisation == null
                    || !Authorisation.HasPermission(Auth.ADMIN_WRITE))
                {
                    role.IsReadOnly = true;
                }

                await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);

                return role;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);

                throw new AtlasException(ex.Message, ex);
            }
        }

        public async Task<Role> UpdateRoleAsync(Role updateRole, CancellationToken cancellationToken)
        {
            try
            {
                Role role = await _applicationDbContext.Roles
                    .Include(r => r.Permissions)
                    .FirstAsync(r => r.RoleId.Equals(updateRole.RoleId), cancellationToken)
                    .ConfigureAwait(false);

                _applicationDbContext
                    .Entry(role)
                    .CurrentValues.SetValues(updateRole);

                List<int> selectedPermissionIds = ExtractSelectedPemissions(updateRole.PermissionChecklist);

                List<Permission> removePermissions = role.Permissions
                    .Where(rp => !selectedPermissionIds.Any(p => p.Equals(rp.PermissionId)))
                    .ToList();

                foreach (Permission permission in removePermissions)
                {
                    role.Permissions.Remove(permission);
                }

                List<int> newPermissionIds = selectedPermissionIds
                    .Where(np => !role.Permissions.Any(rp => rp.PermissionId.Equals(np)))
                    .ToList();

                if (newPermissionIds.Count > 0)
                {
                    IEnumerable<Permission> permissions = await _applicationDbContext.Permissions
                        .AsNoTracking()
                        .Where(p => newPermissionIds.Contains(p.PermissionId))
                        .ToListAsync(cancellationToken);

                    role.Permissions.AddRange(permissions);
                }

                _applicationDbContext.Roles.Update(role);

                await _applicationDbContext
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                role.PermissionChecklist = await GetPermissionChecklistAsync(role.Permissions, cancellationToken)
                    .ConfigureAwait(false);

                if (Authorisation == null
                    || !Authorisation.HasPermission(Auth.ADMIN_WRITE))
                {
                    role.IsReadOnly = true;
                }

                return role;
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex, $"RoleId={updateRole.RoleId}");
            }
        }

        public async Task<int> DeleteRoleAsync(int roleId, CancellationToken cancellationToken)
        {
            try
            {
                Role role = await _applicationDbContext.Roles
                    .FirstAsync(r => r.RoleId.Equals(roleId), cancellationToken)
                    .ConfigureAwait(false);

                _applicationDbContext.Roles.Remove(role);

                return await _applicationDbContext
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex, $"RoleId={roleId}");
            }
        }

        public async Task<List<ChecklistItem>> GetPermissionChecklistAsync(List<Permission> modelPermissions, CancellationToken cancellationToken)
        {
            IEnumerable<Permission> permissions = await GetPermissionsAsync(cancellationToken)
                .ConfigureAwait(false);

            List<ChecklistItem> permissionChecklist = [.. (from p in permissions
                                                       select new ChecklistItem
                                                       {
                                                           Id = p.PermissionId,
                                                           Name = p.Name,
                                                           Description = p.Description
                                                       })
                                       .OrderBy(p => p.Name)];

            _ = (from i in permissionChecklist
                 join p in modelPermissions on i.Id equals p.PermissionId
                 select i.IsChecked = true).ToList();

            return permissionChecklist;
        }

        public async Task<List<ChecklistItem>> GetRoleChecklistAsync(List<Role> modelRoles, CancellationToken cancellationToken)
        {
            IEnumerable<Role> roles = await GetRolesAsync(cancellationToken)
                .ConfigureAwait(false);

            static ChecklistItem createChecklistItem(Role role)
            {
                ChecklistItem checklistItem = new()
                {
                    Id = role.RoleId,
                    Name = role.Name,
                    Description = role.Description
                };

                List<string?> permissions = role.Permissions
                .Select(p => p.Name)
                .ToList();

                checklistItem.SubItems.AddRange(permissions);

                return checklistItem;
            }

            List<ChecklistItem> roleChecklist = [.. (from r in roles
                                 select createChecklistItem(r))
                                 .OrderBy(r => r.Name)];

            _ = (from i in roleChecklist
                 join r in modelRoles on i.Id equals r.RoleId
                 select i.IsChecked = true).ToList();

            return roleChecklist;
        }

        private static List<int> ExtractSelectedPemissions(List<ChecklistItem> permissionChecklist)
        {
            return permissionChecklist
                .Where(p => p.IsChecked)
                .Select(p => p.Id)
                .ToList();
        }

        private static List<Role> ExtractSelectedRoles(List<ChecklistItem> roleChecklist)
        {
            return roleChecklist
                .Where(p => p.IsChecked)
                .Select(p => new Role
                {
                    RoleId = p.Id,
                    Name = p.Name,
                    Description = p.Description
                })
                .ToList();
        }
    }
}

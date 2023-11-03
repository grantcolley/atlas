using Atlas.Core.Dynamic;
using Atlas.Core.Models;
using Atlas.Data.Access.Base;
using Atlas.Data.Access.Context;
using Atlas.Data.Access.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Atlas.Data.Access.Data
{
    public class AdministrationData : AuthorisationData<AdministrationData>, IAdministrationData
    {
        public AdministrationData(ApplicationDbContext applicationDbContext, ILogger<AdministrationData> logger)
            : base(applicationDbContext, logger)
        {
        }

        public async Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken)
        {
            return await _applicationDbContext.Users
                .Include(u => u.Permissions)
                .Include(u => u.Roles)
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<User> GetUserAsync(int userId, CancellationToken cancellationToken)
        {
            User user;

            if (userId.Equals(0))
            {
                user = DynamicTypeCreator<User>.Create();
            }
            else
            {
                user = await _applicationDbContext.Users
                    .Include(u => u.Permissions)
                    .Include(u => u.Roles)
                    .AsNoTracking()
                    .FirstAsync(u => u.UserId.Equals(userId), cancellationToken)
                    .ConfigureAwait(false);
            }

            user.PermissionChecklist = await GetPermissionChecklistAsync(user.Permissions, cancellationToken)
                .ConfigureAwait(false);

            user.RoleChecklist = await GetRoleChecklistAsync(user.Roles, cancellationToken)
                .ConfigureAwait(false);

            return user;
        }

        public async Task<User> CreateUserAsync(User addUser, CancellationToken cancellationToken)
        {
            var user = new User
            {
                UserName = addUser.UserName,
                Email = addUser.Email
            };

            await _applicationDbContext.Users
                .AddAsync(user, cancellationToken)
                .ConfigureAwait(false);

            await _applicationDbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            if (addUser.Permissions.Any()
                || addUser.Roles.Any())
            {
                user.Permissions.AddRange(addUser.Permissions);
                user.Roles.AddRange(addUser.Roles);

                await _applicationDbContext
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                user.PermissionChecklist = await GetPermissionChecklistAsync(user.Permissions, cancellationToken)
                    .ConfigureAwait(false);

                user.RoleChecklist = await GetRoleChecklistAsync(user.Roles, cancellationToken)
                    .ConfigureAwait(false);
            }

            return user;
        }

        public async Task<User> UpdateUserAsync(User updateUser, CancellationToken cancellationToken)
        {
            var user = await _applicationDbContext.Users
                .Include(u => u.Permissions)
                .Include(u => u.Roles)
                .FirstAsync(u => u.UserId.Equals(updateUser.UserId), cancellationToken)
                .ConfigureAwait(false);

            user.UserName = updateUser.UserName;
            user.Email = updateUser.Email;

            var permissions = ExtractSelectedPemissions(updateUser.PermissionChecklist);

            var removePermissions = user.Permissions
                .Where(up => !permissions.Any(p => p.PermissionId.Equals(up.PermissionId)))
                .ToList();

            foreach (var permission in removePermissions)
            {
                user.Permissions.Remove(permission);
            }

            var addPermissions = permissions
                .Where(up => !user.Permissions.Any(p => p.PermissionId.Equals(up.PermissionId)))
                .ToList();

            user.Permissions.AddRange(addPermissions);

            var roles = ExtractSelectedRoles(updateUser.RoleChecklist);

            var removeRoles = user.Roles
                .Where(ur => !roles.Any(r => r.RoleId.Equals(ur.RoleId)))
                .ToList();

            foreach (var role in removeRoles)
            {
                user.Roles.Remove(role);
            }

            var addRoles = roles
                .Where(ur => !user.Roles.Any(r => r.RoleId.Equals(ur.RoleId)))
                .ToList();

            user.Roles.AddRange(addRoles);

            _applicationDbContext.Users.Update(user);

            await _applicationDbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            user.PermissionChecklist = await GetPermissionChecklistAsync(user.Permissions, cancellationToken)
                .ConfigureAwait(false);

            user.RoleChecklist = await GetRoleChecklistAsync(user.Roles, cancellationToken)
                .ConfigureAwait(false);

            return user;
        }

        public async Task<int> DeleteUserAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await _applicationDbContext.Users
                .FirstAsync(u => u.UserId.Equals(userId), cancellationToken)
                .ConfigureAwait(false);

            _applicationDbContext.Users.Remove(user);

            return await _applicationDbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Permission>> GetPermissionsAsync(CancellationToken cancellationToken)
        {
            return await _applicationDbContext.Permissions
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Permission> GetPermissionAsync(int permissionId, CancellationToken cancellationToken)
        {
            var permissions = await _applicationDbContext.Permissions
                .AsNoTracking()
                .Include(p => p.Roles)
                .Include(p => p.Users)
                .FirstAsync(p => p.PermissionId.Equals(permissionId), cancellationToken)
                .ConfigureAwait(false);

            var roles = await _applicationDbContext.Roles
                .Include(r => r.Users)
                .Where(r => r.Permissions.Any(p => p.PermissionId.Equals(permissionId)))
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            var users = roles
                .SelectMany(r => r.Users)
                .ToList();

            users = users
                .Where(u => !permissions.Users.Any(pu => pu.UserId.Equals(u.UserId)))
                .ToList();

            permissions.Users.AddRange(users);

            return permissions;
        }

        public async Task<Permission> CreatePermissionAsync(Permission permission, CancellationToken cancellationToken)
        {
            await _applicationDbContext.Permissions
                .AddAsync(permission, cancellationToken)
                .ConfigureAwait(false);

            await _applicationDbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return permission;
        }

        public async Task<Permission> UpdatePermissionAsync(Permission permission, CancellationToken cancellationToken)
        {
            var existing = await _applicationDbContext.Permissions
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

            return permission;
        }

        public async Task<int> DeletePermissionAsync(int permissionId, CancellationToken cancellationToken)
        {
            var permission = await _applicationDbContext.Permissions
                .FirstAsync(p => p.PermissionId.Equals(permissionId), cancellationToken)
                .ConfigureAwait(false);

            _applicationDbContext.Permissions.Remove(permission);

            return await _applicationDbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Role>> GetRolesAsync(CancellationToken cancellationToken)
        {
            return await _applicationDbContext.Roles
                .Include(p => p.Permissions)
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Role> GetRoleAsync(int roleId, CancellationToken cancellationToken)
        {
            Role role;

            if (roleId.Equals(0))
            {
                role = DynamicTypeCreator<Role>.Create();
            }
            else
            {
                role = await _applicationDbContext.Roles
                    .Include(r => r.Users)
                    .Include(r => r.Permissions)
                    .AsNoTracking()
                    .FirstAsync(r => r.RoleId.Equals(roleId), cancellationToken)
                    .ConfigureAwait(false);
            }

            role.PermissionChecklist = await GetPermissionChecklistAsync(role.Permissions, cancellationToken)
                .ConfigureAwait(false);

            return role;
        }

        public async Task<Role> CreateRoleAsync(Role addRole, CancellationToken cancellationToken)
        {
            var role = new Role
            {
                Name = addRole.Name,
                Description = addRole.Description
            };

            var permissions = ExtractSelectedPemissions(addRole.PermissionChecklist);

            await _applicationDbContext.Roles
                .AddAsync(role, cancellationToken)
                .ConfigureAwait(false);

            await _applicationDbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            if (permissions.Any())
            {
                role.Permissions.AddRange(permissions);

                await _applicationDbContext
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

            role.PermissionChecklist = await GetPermissionChecklistAsync(role.Permissions, cancellationToken)
                .ConfigureAwait(false);

            return role;
        }

        public async Task<Role> UpdateRoleAsync(Role updateRole, CancellationToken cancellationToken)
        {
            var role = await _applicationDbContext.Roles
                .Include(r => r.Permissions)
                .FirstAsync(r => r.RoleId.Equals(updateRole.RoleId), cancellationToken)
                .ConfigureAwait(false);

            role.Name = updateRole.Name;
            role.Description = updateRole.Description;

            var permissions = ExtractSelectedPemissions(updateRole.PermissionChecklist);

            var removePermissions = role.Permissions
                .Where(rp => !permissions.Any(p => p.PermissionId.Equals(rp.PermissionId)))
                .ToList();

            foreach (var permission in removePermissions)
            {
                role.Permissions.Remove(permission);
            }

            var addPermissions = permissions
                .Where(rp => !role.Permissions.Any(p => p.PermissionId.Equals(rp.PermissionId)))
                .ToList();

            role.Permissions.AddRange(addPermissions);

            _applicationDbContext.Roles.Update(role);

            await _applicationDbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            role.PermissionChecklist = await GetPermissionChecklistAsync(role.Permissions, cancellationToken)
                .ConfigureAwait(false);

            return role;
        }

        public async Task<int> DeleteRoleAsync(int roleId, CancellationToken cancellationToken)
        {
            var role = await _applicationDbContext.Roles
                .FirstAsync(r => r.RoleId.Equals(roleId), cancellationToken)
                .ConfigureAwait(false);

            _applicationDbContext.Roles.Remove(role);

            return await _applicationDbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task<List<ChecklistItem>> GetPermissionChecklistAsync(List<Permission> modelPermissions, CancellationToken cancellationToken)
        {
            var permissions = await GetPermissionsAsync(cancellationToken)
                .ConfigureAwait(false);

            var permissionChecklist = (from p in permissions
                                       select new ChecklistItem
                                       {
                                           Id = p.PermissionId,
                                           Name = p.Name,
                                           Description = p.Description
                                       }).ToList();

            _ = (from i in permissionChecklist
                 join p in modelPermissions on i.Id equals p.PermissionId
                 select i.IsChecked = true).ToList();

            return permissionChecklist;
        }

        private async Task<List<ChecklistItem>> GetRoleChecklistAsync(List<Role> modelRoles, CancellationToken cancellationToken)
        {
            var roles = await GetRolesAsync(cancellationToken)
                .ConfigureAwait(false);

            static ChecklistItem createChecklistItem(Role role)
            {
                var checklistItem = new ChecklistItem
                {
                    Id = role.RoleId,
                    Name = role.Name,
                    Description = role.Description
                };

                var permissions = role.Permissions
                .Select(p => p.Name)
                .ToList();

                checklistItem.SubItems.AddRange(permissions);

                return checklistItem;
            }

            var roleChecklist = (from r in roles
                                 select createChecklistItem(r)).ToList();

            _ = (from i in roleChecklist
                 join r in modelRoles on i.Id equals r.RoleId
                 select i.IsChecked = true).ToList();

            return roleChecklist;
        }

        private static List<Permission> ExtractSelectedPemissions(List<ChecklistItem> permissionChecklist)
        {
            return permissionChecklist
                .Where(p => p.IsChecked)
                .Select(p => new Permission
                {
                    PermissionId = p.Id,
                    Name = p.Name,
                    Description = p.Description
                })
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

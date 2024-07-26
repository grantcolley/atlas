﻿using Atlas.Core.Constants;
using Atlas.Core.Dynamic;
using Atlas.Core.Models;
using Atlas.Data.Access.Base;
using Atlas.Data.Access.Context;
using Atlas.Data.Access.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Atlas.Data.Access.Data
{
    public class AdministrationData(ApplicationDbContext applicationDbContext, ILogger<AdministrationData> logger) 
        : AuthorisationData<AdministrationData>(applicationDbContext, logger), IAdministrationData
    {
        public async Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken)
        {
            return await _applicationDbContext.Users
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<User> GetUserAsync(int userId, CancellationToken cancellationToken)
        {
            User user = await _applicationDbContext.Users
                    .Include(u => u.Roles)
                    .AsNoTracking()
                    .FirstAsync(u => u.UserId.Equals(userId), cancellationToken)
                    .ConfigureAwait(false);

            user.RoleChecklist = await GetRoleChecklistAsync(user.Roles, cancellationToken)
                .ConfigureAwait(false);

            if(Authorisation == null
                || !Authorisation.HasPermission(Auth.ADMIN_WRITE))
            {
                user.IsReadOnly = true;
            }

            return user;
        }

        public async Task<User> CreateUserAsync(User addUser, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(addUser));
            if (addUser.UserId != 0) throw new ArgumentException(nameof(addUser.UserId), $"Cannot create new user with UserId {addUser.UserId}");

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

            if (addUser.Roles.Count > 0)
            {
                user.Roles.AddRange(addUser.Roles);

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

            return user;
        }

        public async Task<User> UpdateUserAsync(User updateUser, CancellationToken cancellationToken)
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

        public async Task<int> DeleteUserAsync(int userId, CancellationToken cancellationToken)
        {
            User user = await _applicationDbContext.Users
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

        public async Task<Permission> CreatePermissionAsync(Permission permission, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(permission));
            if(permission.PermissionId != 0) throw new ArgumentException(nameof(permission.PermissionId), $"Cannot create new permission with PermissionId {permission.PermissionId}");

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

        public async Task<Permission> UpdatePermissionAsync(Permission permission, CancellationToken cancellationToken)
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

        public async Task<int> DeletePermissionAsync(int permissionId, CancellationToken cancellationToken)
        {
            Permission permission = await _applicationDbContext.Permissions
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

        public async Task<Role> CreateRoleAsync(Role addRole, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(addRole));
            if (addRole.RoleId != 0) throw new ArgumentException(nameof(addRole.RoleId), $"Cannot create new role with RoleId {addRole.RoleId}");

            Role role = new()
            {
                Name = addRole.Name,
                Description = addRole.Description
            };

            List<Permission> permissions = ExtractSelectedPemissions(role.PermissionChecklist);

            await _applicationDbContext.Roles
                .AddAsync(role, cancellationToken)
                .ConfigureAwait(false);

            await _applicationDbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            if (permissions.Count > 0)
            {
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

            return role;
        }

        public async Task<Role> UpdateRoleAsync(Role updateRole, CancellationToken cancellationToken)
        {
            Role role = await _applicationDbContext.Roles
                .Include(r => r.Permissions)
                .FirstAsync(r => r.RoleId.Equals(updateRole.RoleId), cancellationToken)
                .ConfigureAwait(false);

            _applicationDbContext
                .Entry(role)
                .CurrentValues.SetValues(updateRole);

            List<Permission> permissions = ExtractSelectedPemissions(updateRole.PermissionChecklist);

            List<Permission> removePermissions = role.Permissions
                .Where(rp => !permissions.Any(p => p.PermissionId.Equals(rp.PermissionId)))
                .ToList();

            foreach (Permission permission in removePermissions)
            {
                role.Permissions.Remove(permission);
            }

            List<Permission> addPermissions = permissions
                .Where(rp => !role.Permissions.Any(p => p.PermissionId.Equals(rp.PermissionId)))
                .ToList();

            role.Permissions.AddRange(addPermissions);

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

        public async Task<int> DeleteRoleAsync(int roleId, CancellationToken cancellationToken)
        {
            Role role = await _applicationDbContext.Roles
                .FirstAsync(r => r.RoleId.Equals(roleId), cancellationToken)
                .ConfigureAwait(false);

            _applicationDbContext.Roles.Remove(role);

            return await _applicationDbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task<List<ChecklistItem>> GetPermissionChecklistAsync(List<Permission> modelPermissions, CancellationToken cancellationToken)
        {
            IEnumerable<Permission> permissions = await GetPermissionsAsync(cancellationToken)
                .ConfigureAwait(false);

            List<ChecklistItem> permissionChecklist = (from p in permissions
                                       select new ChecklistItem
                                       {
                                           Id = p.PermissionId,
                                           Name = p.Name,
                                           Description = p.Description
                                       })
                                       .OrderBy(p => p.Name)
                                       .ToList();

            _ = (from i in permissionChecklist
                 join p in modelPermissions on i.Id equals p.PermissionId
                 select i.IsChecked = true).ToList();

            return permissionChecklist;
        }

        private async Task<List<ChecklistItem>> GetRoleChecklistAsync(List<Role> modelRoles, CancellationToken cancellationToken)
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

﻿using Atlas.Core.Models;

namespace Atlas.Data.Access.Interfaces
{
    public interface IAdministrationData : IAuthorisationData
    {
        Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken);
        Task<User> GetUserAsync(int userId, CancellationToken cancellationToken);
        Task<User> AddUserAsync(User user, CancellationToken cancellationToken);
        Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken);
        Task<int> DeleteUserAsync(int userId, CancellationToken cancellationToken);
        Task<IEnumerable<Permission>> GetPermissionsAsync(CancellationToken cancellationToken);
        Task<Permission> GetPermissionAsync(int permissionId, CancellationToken cancellationToken);
        Task<Permission> AddPermissionAsync(Permission permission, CancellationToken cancellationToken);
        Task<Permission> UpdatePermissionAsync(Permission permission, CancellationToken cancellationToken);
        Task<int> DeletePermissionAsync(int permissionId, CancellationToken cancellationToken);
        Task<IEnumerable<Role>> GetRolesAsync(CancellationToken cancellationToken);
        Task<Role> GetRoleAsync(int roleId, CancellationToken cancellationToken);
        Task<Role> AddRoleAsync(Role role, CancellationToken cancellationToken);
        Task<Role> UpdateRoleAsync(Role role, CancellationToken cancellationToken);
        Task<int> DeleteRoleAsync(int roleId, CancellationToken cancellationToken);
    }
}

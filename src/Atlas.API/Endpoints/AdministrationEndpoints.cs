using Atlas.API.Interfaces;
using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Atlas.API.Endpoints
{
    internal static class AdministrationEndpoints
    {
        internal static async Task<IResult> GetUsers(IAdministrationData administrationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN))
                {
                    return Results.Unauthorized();
                }

                IEnumerable<User>? users = await administrationData.GetUsersAsync(cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(users);
            }
            catch (Exception)
            {
                // Exceptions thrown from administrationData.GetUsersAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetUser(int id, IAdministrationData administrationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN))
                {
                    return Results.Unauthorized();
                }

                User? user = await administrationData.GetUserAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(user);
            }
            catch (Exception)
            {
                // Exceptions thrown from administrationData.GetUserAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> CreateUser([FromBody] User user, IAdministrationData administrationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN))
                {
                    return Results.Unauthorized();
                }

                User? newUser = await administrationData.CreateUserAsync(user, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(newUser);
            }
            catch (Exception)
            {
                // Exceptions thrown from administrationData.CreateUserAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> UpdateUser([FromBody] User user, IAdministrationData administrationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN))
                {
                    return Results.Unauthorized();
                }

                User? updatedUser = await administrationData.UpdateUserAsync(user, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(updatedUser);
            }
            catch (Exception)
            {
                // Exceptions thrown from administrationData.UpdateUserAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> DeleteUser(int id, IAdministrationData administrationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN))
                {
                    return Results.Unauthorized();
                }

                int affectedRows = await administrationData.DeleteUserAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(affectedRows);
            }
            catch (Exception)
            {
                // Exceptions thrown from administrationData.DeleteUserAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetRoles(IAdministrationData administrationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN))
                {
                    return Results.Unauthorized();
                }

                IEnumerable<Role>? roles = await administrationData.GetRolesAsync(cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(roles);
            }
            catch (Exception)
            {
                // Exceptions thrown from administrationData.GetRolesAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetRole(int id, IAdministrationData administrationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN))
                {
                    return Results.Unauthorized();
                }

                Role? role = await administrationData.GetRoleAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(role);
            }
            catch (Exception)
            {
                // Exceptions thrown from administrationData.GetRoleAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> CreateRole([FromBody] Role role, IAdministrationData administrationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN))
                {
                    return Results.Unauthorized();
                }

                Role? newRole = await administrationData.CreateRoleAsync(role, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(newRole);
            }
            catch (Exception)
            {
                // Exceptions thrown from administrationData.CreateRoleAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> UpdateRole([FromBody] Role role, IAdministrationData administrationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN))
                {
                    return Results.Unauthorized();
                }

                Role? updatedRole = await administrationData.UpdateRoleAsync(role, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(updatedRole);
            }
            catch (Exception)
            {
                // Exceptions thrown from administrationData.UpdateRoleAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> DeleteRole(int id, IAdministrationData administrationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN))
                {
                    return Results.Unauthorized();
                }

                int affectedRows = await administrationData.DeleteRoleAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(affectedRows);
            }
            catch (Exception)
            {
                // Exceptions thrown from administrationData.DeleteRoleAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetPermissions(IAdministrationData administrationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN))
                {
                    return Results.Unauthorized();
                }

                IEnumerable<Permission>? permissions = await administrationData.GetPermissionsAsync(cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(permissions);
            }
            catch (Exception)
            {
                // Exceptions thrown from administrationData.GetPermissionAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetPermission(int id, IAdministrationData administrationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN))
                {
                    return Results.Unauthorized();
                }

                Permission? permission = await administrationData.GetPermissionAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(permission);
            }
            catch (Exception)
            {
                // Exceptions thrown from administrationData.GetPermissionAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> CreatePermission([FromBody] Permission permission, IAdministrationData administrationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN))
                {
                    return Results.Unauthorized();
                }

                Permission? newPermission = await administrationData.CreatePermissionAsync(permission, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(newPermission);
            }
            catch (Exception)
            {
                // Exceptions thrown from administrationData.CreatePermissionAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> UpdatePermission([FromBody] Permission permission, IAdministrationData administrationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN))
                {
                    return Results.Unauthorized();
                }

                Permission? updatedPermission = await administrationData.UpdatePermissionAsync(permission, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(updatedPermission);
            }
            catch (Exception)
            {
                // Exceptions thrown from administrationData.UpdatePermissionAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> DeletePermission(int id, IAdministrationData administrationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN))
                {
                    return Results.Unauthorized();
                }

                int affectedRows = await administrationData.DeletePermissionAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(affectedRows);
            }
            catch (Exception)
            {
                // Exceptions thrown from administrationData.DeletePermissionAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

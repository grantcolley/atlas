using Atlas.API.Interfaces;
using Atlas.API.Utility;
using Atlas.Core.Constants;
using Atlas.Core.Exceptions;
using Atlas.Core.Logging.Interfaces;
using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Atlas.API.Endpoints
{
    internal static class AdministrationEndpoints
    {
        internal static async Task<IResult> GetUsers(IAdministrationData administrationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN_READ))
                {
                    return Results.Unauthorized();
                }

                IEnumerable<User>? users = await administrationData.GetUsersAsync(cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(new AuthResult<IEnumerable<User>?> { Authorisation = authorisation, Result = users });
            }
            catch (AtlasException ex)
            {
                logService.Log(Core.Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetUser(int id, IAdministrationData administrationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN_READ))
                {
                    return Results.Unauthorized();
                }

                User? user = await administrationData.GetUserAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(user);
            }
            catch (AtlasException ex)
            {
                logService.Log(Core.Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> CreateUser([FromBody] User user, IValidator<User> validator, IAdministrationData administrationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN_WRITE))
                {
                    return Results.Unauthorized();
                }

                await validator.ValidateAndThrowAtlasException(user, "CreateUser", cancellationToken).ConfigureAwait(false);

                User? newUser = await administrationData.CreateUserAsync(user, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(newUser);
            }
            catch (AtlasException ex)
            {
                logService.Log(Core.Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> UpdateUser([FromBody] User user, IValidator<User> validator, IAdministrationData administrationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN_WRITE))
                {
                    return Results.Unauthorized();
                }

                await validator.ValidateAndThrowAtlasException(user, "UpdateUser", cancellationToken).ConfigureAwait(false);

                User? updatedUser = await administrationData.UpdateUserAsync(user, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(updatedUser);
            }
            catch (AtlasException ex)
            {
                logService.Log(Core.Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> DeleteUser(int id, IAdministrationData administrationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN_WRITE))
                {
                    return Results.Unauthorized();
                }

                int affectedRows = await administrationData.DeleteUserAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(affectedRows);
            }
            catch (AtlasException ex)
            {
                logService.Log(Core.Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetRoles(IAdministrationData administrationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN_READ))
                {
                    return Results.Unauthorized();
                }

                IEnumerable<Role>? roles = await administrationData.GetRolesAsync(cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(new AuthResult<IEnumerable<Role>?> { Authorisation = authorisation, Result = roles });
            }
            catch (AtlasException ex)
            {
                logService.Log(Core.Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetRole(int id, IAdministrationData administrationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN_READ))
                {
                    return Results.Unauthorized();
                }

                Role? role = await administrationData.GetRoleAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(role);
            }
            catch (AtlasException ex)
            {
                logService.Log(Core.Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> CreateRole([FromBody] Role role, IValidator<Role> validator, IAdministrationData administrationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN_WRITE))
                {
                    return Results.Unauthorized();
                }

                await validator.ValidateAndThrowAtlasException(role, "CreateRole", cancellationToken).ConfigureAwait(false);

                Role? newRole = await administrationData.CreateRoleAsync(role, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(newRole);
            }
            catch (AtlasException ex)
            {
                logService.Log(Core.Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> UpdateRole([FromBody] Role role, IValidator<Role> validator, IAdministrationData administrationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN_WRITE))
                {
                    return Results.Unauthorized();
                }

                await validator.ValidateAndThrowAtlasException(role, "UpdateRole", cancellationToken).ConfigureAwait(false);

                Role? updatedRole = await administrationData.UpdateRoleAsync(role, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(updatedRole);
            }
            catch (AtlasException ex)
            {
                logService.Log(Core.Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> DeleteRole(int id, IAdministrationData administrationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN_WRITE))
                {
                    return Results.Unauthorized();
                }

                int affectedRows = await administrationData.DeleteRoleAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(affectedRows);
            }
            catch (AtlasException ex)
            {
                logService.Log(Core.Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetPermissions(IAdministrationData administrationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN_READ))
                {
                    return Results.Unauthorized();
                }

                IEnumerable<Permission>? permissions = await administrationData.GetPermissionsAsync(cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(new AuthResult<IEnumerable<Permission>?> { Authorisation = authorisation, Result = permissions });
            }
            catch (AtlasException ex)
            {
                logService.Log(Core.Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetPermission(int id, IAdministrationData administrationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN_READ))
                {
                    return Results.Unauthorized();
                }

                Permission? permission = await administrationData.GetPermissionAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(permission);
            }
            catch (AtlasException ex)
            {
                logService.Log(Core.Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> CreatePermission([FromBody] Permission permission, IValidator<Permission> validator, IAdministrationData administrationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN_WRITE))
                {
                    return Results.Unauthorized();
                }

                await validator.ValidateAndThrowAtlasException(permission, "CreatePermission", cancellationToken).ConfigureAwait(false);

                Permission? newPermission = await administrationData.CreatePermissionAsync(permission, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(newPermission);
            }
            catch (AtlasException ex)
            {
                logService.Log(Core.Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> UpdatePermission([FromBody] Permission permission, IValidator<Permission> validator, IAdministrationData administrationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN_WRITE))
                {
                    return Results.Unauthorized();
                }

                await validator.ValidateAndThrowAtlasException(permission, "UpdatePermission", cancellationToken).ConfigureAwait(false);

                Permission? updatedPermission = await administrationData.UpdatePermissionAsync(permission, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(updatedPermission);
            }
            catch (AtlasException ex)
            {
                logService.Log(Core.Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> DeletePermission(int id, IAdministrationData administrationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN_WRITE))
                {
                    return Results.Unauthorized();
                }

                int affectedRows = await administrationData.DeletePermissionAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(affectedRows);
            }
            catch (AtlasException ex)
            {
                logService.Log(Core.Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetPermissionChecklistAsync(IAdministrationData administrationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN_WRITE))
                {
                    return Results.Unauthorized();
                }

                IEnumerable<ChecklistItem>? permissions = await administrationData.GetPermissionChecklistAsync([], cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(new AuthResult<IEnumerable<ChecklistItem>?> { Authorisation = authorisation, Result = permissions });
            }
            catch (AtlasException ex)
            {
                logService.Log(Core.Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetRoleChecklistAsync(IAdministrationData administrationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await administrationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN_WRITE))
                {
                    return Results.Unauthorized();
                }

                IEnumerable<ChecklistItem>? roles = await administrationData.GetRoleChecklistAsync([], cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(new AuthResult<IEnumerable<ChecklistItem>?> { Authorisation = authorisation, Result = roles });
            }
            catch (AtlasException ex)
            {
                logService.Log(Core.Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

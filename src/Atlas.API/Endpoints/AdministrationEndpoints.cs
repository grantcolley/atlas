using Atlas.API.Interfaces;
using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Atlas.API.Endpoints
{
    internal static class AdministrationEndpoints
    {
        internal static async Task<IResult> GetModules(INavigationData navigationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await navigationData.GetAuthorisationAsync(claimService.GetClaim())
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN))
                {
                    return Results.Unauthorized();
                }

                IEnumerable<Module>? modules = await navigationData.GetModulesAsync(cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(modules);
            }
            catch (Exception)
            {
                // Exceptions thrown from navigationData.GetModulesAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetModule(int id, INavigationData navigationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await navigationData.GetAuthorisationAsync(claimService.GetClaim())
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN))
                {
                    return Results.Unauthorized();
                }

                Module? module = await navigationData.GetModuleAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(module);
            }
            catch (Exception)
            {
                // Exceptions thrown from navigationData.GetModuleAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> CreateModule([FromBody] Module module, INavigationData navigationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await navigationData.GetAuthorisationAsync(claimService.GetClaim())
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN))
                {
                    return Results.Unauthorized();
                }

                Module? newModule = await navigationData.CreateModuleAsync(module, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(newModule);
            }
            catch (Exception)
            {
                // Exceptions thrown from navigationData.CreateModuleAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> UpdateModule([FromBody] Module module, INavigationData navigationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await navigationData.GetAuthorisationAsync(claimService.GetClaim())
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN))
                {
                    return Results.Unauthorized();
                }

                Module? updatedModule = await navigationData.UpdateModuleAsync(module, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(updatedModule);
            }
            catch (Exception)
            {
                // Exceptions thrown from navigationData.UpdateModuleAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> DeleteModule(int id, INavigationData navigationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await navigationData.GetAuthorisationAsync(claimService.GetClaim())
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.ADMIN))
                {
                    return Results.Unauthorized();
                }

                int affectedRows = await navigationData.DeleteModuleAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(affectedRows);
            }
            catch (Exception)
            {
                // Exceptions thrown from navigationData.DeleteModuleAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

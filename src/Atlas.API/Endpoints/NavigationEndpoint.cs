using Atlas.API.Interfaces;
using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;

namespace Atlas.API.Endpoints
{
    internal static class NavigationEndpoint
    {
        internal static async Task<IResult> GetClaimModules(INavigationData navigationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await navigationData.GetAuthorisationAsync(claimService.GetClaim())
                    .ConfigureAwait(false);

                if (authorisation == null
                    || string.IsNullOrWhiteSpace(authorisation.User))
                {
                    return Results.Unauthorized();
                }

                IEnumerable<Module>? modules = await navigationData.GetNavigationClaimsAsync(authorisation.User, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(modules);
            }
            catch (Exception)
            {
                // Exceptions thrown from navigationData.GetNavigationClaimsAsync(token)
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

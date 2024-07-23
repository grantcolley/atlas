using Atlas.API.Interfaces;
using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;

namespace Atlas.API.Endpoints
{
    internal static class ClaimEndpoint
    {
        internal static async Task<IResult> GetClaimAuthorisation(IClaimData userData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await userData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || string.IsNullOrWhiteSpace(authorisation.User))
                {
                    return Results.Unauthorized();
                }

                return Results.Ok(authorisation);
            }
            catch (Exception)
            {
                // Exceptions thrown from userData.GetAuthorisationAsync(claim)
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetClaimModules(IClaimData userData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await userData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || string.IsNullOrWhiteSpace(authorisation.User))
                {
                    return Results.Unauthorized();
                }

                IEnumerable<Module>? modules = await userData.GetNavigationClaimsAsync(cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(modules);
            }
            catch (Exception)
            {
                // Exceptions thrown from userData.GetNavigationClaimsAsync(claim)
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

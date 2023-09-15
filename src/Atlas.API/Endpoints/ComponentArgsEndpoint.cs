using Atlas.API.Interfaces;
using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;

namespace Atlas.API.Endpoints
{
    internal static class ComponentArgsEndpoint
    {
        internal static async Task<IResult> GetComponentArgs(string? componentCode, IComponentArgsData componentArgsData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await componentArgsData.GetAuthorisationAsync(claimService.GetClaim())
                    .ConfigureAwait(false);

                if (authorisation == null
                    || string.IsNullOrWhiteSpace(authorisation.User))
                {
                    return Results.Unauthorized();
                }

                ComponentArgs? componentArgs = await componentArgsData.GetComponentArgsAsync(componentCode, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(componentArgs);
            }
            catch (Exception)
            {
                // Exceptions thrown from componentArgsData.GetComponentArgsDataAsync(token)
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

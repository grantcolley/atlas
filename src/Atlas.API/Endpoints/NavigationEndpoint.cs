using Atlas.API.Interfaces;
using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;

namespace Atlas.API.Endpoints
{
    internal static class NavigationEndpoint
    {
        internal static async Task<IResult> GetClaimModules(IAuthorisationService authorisationService, INavigationData navigationData, CancellationToken cancellationToken)
        {
            try
            {
                var authorisation = await authorisationService.GetAuthorisationAsync()
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

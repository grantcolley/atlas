using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;

namespace Atlas.API.Endpoints
{
    internal static class NavigationEndpoint
    {
        internal static async Task<IResult> GetNavigationClaims(INavigationData navigationData, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<Module>? modules = await navigationData.GetNavigationClaimsAsync("", cancellationToken)
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

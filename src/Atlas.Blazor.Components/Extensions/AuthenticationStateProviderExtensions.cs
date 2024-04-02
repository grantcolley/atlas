using Atlas.Core.Constants;
using Microsoft.AspNetCore.Components.Authorization;

namespace Atlas.Blazor.Components.Extensions
{
    public static class AuthenticationStateProviderExtensions
    {
        public static async Task<bool> IsAuthenticatedAtlasUser(this AuthenticationStateProvider authenticationStateProvider)
        {
            if (authenticationStateProvider == null)
            {
                return false;
            }

            var authState = await authenticationStateProvider.GetAuthenticationStateAsync().ConfigureAwait(false);

            if (authState == null
                || authState.User == null
                || authState.User.Identity == null
                || !authState.User.Identity.IsAuthenticated)
            {
                return false;
            }

            return authState.User.HasClaim(c => c.Value.Equals(Auth.ATLAS_USER_CLAIM));
        }

        public static async Task<string?> AuthenticatedAtlasUserName(this AuthenticationStateProvider authenticationStateProvider)
        {
            if (authenticationStateProvider == null)
            {
                return null;
            }

            var authState = await authenticationStateProvider.GetAuthenticationStateAsync().ConfigureAwait(false);

            if (authState == null
                || authState.User == null
                || authState.User.Identity == null
                || !authState.User.Identity.IsAuthenticated)
            {
                return null;
            }

            return authState.User.Identity.Name;
        }
    }
}

using Microsoft.AspNetCore.Components.Authorization;

namespace Atlas.Blazor.UI.Extensions
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

            return authState.User.HasClaim(c => c.Value.Equals(Core.Constants.Auth.ATLAS_USER_CLAIM));
        }
    }
}

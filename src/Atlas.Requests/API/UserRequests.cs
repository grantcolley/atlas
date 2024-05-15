using Atlas.Core.Authentication;
using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Requests.Base;
using Atlas.Requests.Interfaces;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

namespace Atlas.Requests.API
{
    public class UserRequests : RequestBase, IUserRequests
    {
        public UserRequests(HttpClient httpClient)
            : base(httpClient)
        {
        }

        public async Task<IEnumerable<Module>?> GetClaimModulesAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Module>?>
                (await _httpClient.GetStreamAsync(AtlasAPIEndpoints.GET_CLAIM_MODULES)
                .ConfigureAwait(false),
                new JsonSerializerOptions(JsonSerializerDefaults.Web))
                .ConfigureAwait(false);
        }

        public async Task<string?> GetThemeAsync()
        {
            return await JsonSerializer.DeserializeAsync<string?>
                (await _httpClient.GetStreamAsync(AtlasAPIEndpoints.GET_THEME)
                .ConfigureAwait(false),
                new JsonSerializerOptions(JsonSerializerDefaults.Web))
                .ConfigureAwait(false);
        }

        public async Task<IResponse<bool>> SetThemeAsync(string theme)
        {
            using var response = await _httpClient.PostAsJsonAsync(AtlasAPIEndpoints.SET_THEME, theme)
                .ConfigureAwait(false);

            return await GetResponseAsync<bool>(response)
                .ConfigureAwait(false);
        }

        public async Task<Authorisation?> GetAuthorisationAsync(ClaimsPrincipal principal)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));

            if(!principal.HasClaim(c => c.Value.Equals(Core.Constants.Auth.ATLAS_USER_CLAIM)))
            {
                Claim? id = principal.FindFirst(ClaimTypes.Email);

                return await JsonSerializer.DeserializeAsync<Authorisation?>
                    (await _httpClient.GetStreamAsync(AtlasAPIEndpoints.GET_CLAIM_AUTHORIZATION)
                    .ConfigureAwait(false),
                    new JsonSerializerOptions(JsonSerializerDefaults.Web))
                    .ConfigureAwait(false);
            }

            return await Task.FromResult<Authorisation?>(null);
        }
    }
}

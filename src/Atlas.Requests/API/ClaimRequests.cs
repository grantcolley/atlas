using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Requests.Interfaces;
using System.Security.Claims;
using System.Text.Json;

namespace Atlas.Requests.API
{
    public class ClaimRequests : IClaimRequests
    {
        protected readonly HttpClient _httpClient;

        public ClaimRequests(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<Module>?> GetClaimModulesAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Module>?>
                (await _httpClient.GetStreamAsync(AtlasAPIEndpoints.GET_CLAIM_MODULES)
                .ConfigureAwait(false),
                new JsonSerializerOptions(JsonSerializerDefaults.Web))
                .ConfigureAwait(false);
        }

        public async Task<Authorisation?> GetAuthorisationAsync(ClaimsPrincipal principal)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));

            if(!principal.HasClaim(c => c.Value.Equals(Auth.ATLAS_USER_CLAIM)))
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

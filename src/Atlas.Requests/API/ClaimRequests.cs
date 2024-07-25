using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Requests.Interfaces;
using System.Security.Claims;
using System.Text.Json;

namespace Atlas.Requests.API
{
    public class ClaimRequests(HttpClient httpClient) : IClaimRequests
    {
        protected readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        protected readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web);

        public async Task<IEnumerable<Module>?> GetClaimModulesAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Module>?>
                (await _httpClient.GetStreamAsync(AtlasAPIEndpoints.GET_CLAIM_MODULES)
                .ConfigureAwait(false), _jsonSerializerOptions).ConfigureAwait(false);
        }

        public async Task<Authorisation?> GetAuthorisationAsync(ClaimsPrincipal principal)
        {
            ArgumentNullException.ThrowIfNull(principal);

            if (!principal.HasClaim(c => c.Value.Equals(Auth.ATLAS_USER_CLAIM)))
            {
                Claim? id = principal.FindFirst(ClaimTypes.Email);

                return await JsonSerializer.DeserializeAsync<Authorisation?>
                    (await _httpClient.GetStreamAsync(AtlasAPIEndpoints.GET_CLAIM_AUTHORIZATION)
                    .ConfigureAwait(false), _jsonSerializerOptions).ConfigureAwait(false);
            }

            return await Task.FromResult<Authorisation?>(null);
        }
    }
}

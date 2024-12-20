using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Requests.Base;
using Atlas.Requests.Interfaces;

namespace Atlas.Requests.API
{
    public class ClaimRequests(HttpClient httpClient) : RequestBase(httpClient), IClaimRequests
    {
        public async Task<IResponse<IEnumerable<Module>?>> GetClaimModulesAsync()
        {
            using HttpResponseMessage httpResponseMessage 
                = await _httpClient.GetAsync(AtlasAPIEndpoints.GET_CLAIM_MODULES).ConfigureAwait(false);

            IResponse<IEnumerable<Module>?> response = await GetResponseAsync<IEnumerable<Module>?>(httpResponseMessage)
                .ConfigureAwait(false);

            return response;
        }
    }
}

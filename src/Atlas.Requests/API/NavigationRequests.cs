using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Requests.Base;
using Atlas.Requests.Interfaces;
using System.Text.Json;

namespace Atlas.Requests.API
{
    public class NavigationRequests : RequestBase, INavigationRequests
    {
        public NavigationRequests(HttpClient httpClient)
            : base(httpClient)
        {
        }

        public NavigationRequests(HttpClient httpClient, TokenProvider tokenProvider)
            : base(httpClient, tokenProvider)
        {
        }

        public async Task<IEnumerable<Module>?> GetClaimModulesAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Module>?>
                (await _httpClient.GetStreamAsync(AtlasAPIEndpoints.CLAIM_MODULES)
                .ConfigureAwait(false),
                new JsonSerializerOptions(JsonSerializerDefaults.Web))
                .ConfigureAwait(false);
        }
    }
}

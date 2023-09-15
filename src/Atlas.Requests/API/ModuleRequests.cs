using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Requests.Base;
using Atlas.Requests.Interfaces;
using System.Text.Json;

namespace Atlas.Requests.API
{
    public class ModuleRequests : RequestBase, IModuleRequests
    {
        public ModuleRequests(HttpClient httpClient)
            : base(httpClient)
        {
        }

        public ModuleRequests(HttpClient httpClient, TokenProvider tokenProvider)
            : base(httpClient, tokenProvider)
        {
        }

        public async Task<IEnumerable<Module>?> GetModulesAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Module>?>
                (await _httpClient.GetStreamAsync(AtlasAPIEndpoints.CLAIM_MODULES).ConfigureAwait(false),
                new JsonSerializerOptions(JsonSerializerDefaults.Web)).ConfigureAwait(false);
        }
    }
}

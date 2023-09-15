using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Requests.Base;
using Atlas.Requests.Interfaces;
using System.Text.Json;

namespace Atlas.Requests.API
{
    public class ComponentArgsRequests : RequestBase, IComponentArgsRequests
    {
        public ComponentArgsRequests(HttpClient httpClient)
            : base(httpClient)
        {
        }

        public ComponentArgsRequests(HttpClient httpClient, TokenProvider tokenProvider)
            : base(httpClient, tokenProvider)
        {
        }

        public async Task<ComponentArgs?> GetComponentArgsAsync(string? componentCode)
        {
            if(componentCode == null) throw new ArgumentNullException(nameof(componentCode));

            return await JsonSerializer.DeserializeAsync<ComponentArgs?>
                (await _httpClient.GetStreamAsync($"{AtlasAPIEndpoints.COMPONENT_ARGS}/{componentCode}").ConfigureAwait(false),
                new JsonSerializerOptions(JsonSerializerDefaults.Web)).ConfigureAwait(false);
        }
    }
}

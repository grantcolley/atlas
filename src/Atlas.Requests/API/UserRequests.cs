using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Requests.Base;
using Atlas.Requests.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace Atlas.Requests.API
{
    public class UserRequests : RequestBase, IUserRequests
    {
        public UserRequests(HttpClient httpClient)
            : base(httpClient)
        {
        }

        public UserRequests(HttpClient httpClient, TokenProvider tokenProvider)
            : base(httpClient, tokenProvider)
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

        public async Task SetThemeAsync(string theme)
        {
            await _httpClient.PostAsJsonAsync(AtlasAPIEndpoints.SET_THEME, theme)
                .ConfigureAwait(false);
        }
    }
}

using Atlas.Core.Authentication;
using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Requests.Base;
using Atlas.Requests.Interfaces;
using System.Net.Http.Json;

namespace Atlas.Requests.API
{
    public class OptionsRequest : RequestBase, IOptionsRequest
    {
        public OptionsRequest(HttpClient httpClient)
            : base(httpClient)
        {
        }

        public OptionsRequest(HttpClient httpClient, TokenProvider tokenProvider)
            : base(httpClient, tokenProvider)
        {
        }

        public async Task<IResponse<IEnumerable<OptionItem>?>> GetOptionItems(IEnumerable<OptionsArg> optionsArgs)
        {
            using var response = await _httpClient.PostAsJsonAsync(AtlasAPIEndpoints.GET_OPTIONS, optionsArgs)
                .ConfigureAwait(false);

            return await GetResponseAsync<IEnumerable<OptionItem>?>(response)
                .ConfigureAwait(false);
        }
    }
}

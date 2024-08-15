using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Requests.Base;
using Atlas.Requests.Interfaces;
using System.Net.Http.Json;

namespace Atlas.Requests.API
{
    public class OptionsRequests(HttpClient httpClient) : RequestBase(httpClient), IOptionsRequests
    {
        public async Task<IResponse<IEnumerable<OptionItem>?>> GetOptionItemsAsync(string optionsCode)
        {
            List<OptionsArg> options = [new() { Name = Options.OPTIONS_CODE, Value = optionsCode }];
            return await GetOptionItemsAsync(options).ConfigureAwait(false);
        }

        public async Task<IResponse<IEnumerable<OptionItem>?>> GetOptionItemsAsync(IEnumerable<OptionsArg> optionsArgs)
        {
            using HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync(AtlasAPIEndpoints.GET_OPTIONS, optionsArgs)
                .ConfigureAwait(false);

            return await GetResponseAsync<IEnumerable<OptionItem>?>(httpResponseMessage)
                .ConfigureAwait(false);
        }

        public async Task<IResponse<IEnumerable<T>?>> GetGenericOptionsAsync<T>(string optionsCode)
        {
            List<OptionsArg> options = [new() { Name = Options.OPTIONS_CODE, Value = optionsCode }];
            return await GetGenericOptionsAsync<T>(options).ConfigureAwait(false);
        }

        public async Task<IResponse<IEnumerable<T>?>> GetGenericOptionsAsync<T>(IEnumerable<OptionsArg> optionsArgs)
        {
            using HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync(AtlasAPIEndpoints.GET_GENERIC_OPTIONS, optionsArgs)
                .ConfigureAwait(false);

            return await GetResponseAsync<IEnumerable<T>?>(httpResponseMessage)
                .ConfigureAwait(false);
        }
    }
}

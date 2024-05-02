using Atlas.Core.Authentication;
using Atlas.Core.Constants;
using Atlas.Core.Extensions;
using Atlas.Core.Interfaces;
using Atlas.Core.Models;
using Atlas.Core.Options;
using Atlas.Requests.Base;
using Atlas.Requests.Interfaces;
using Atlas.Requests.Model;
using System.Net.Http.Json;

namespace Atlas.Requests.API
{
    public class OptionsRequest : RequestBase, IOptionsRequest
    {
        private readonly Dictionary<string, IOptionItems> _localOptionItems = new();

        public OptionsRequest(HttpClient httpClient)
            : base(httpClient)
        {
            _localOptionItems.Add(Options.PAGE_CODES, new PageCodeOptionItems());
            _localOptionItems.Add(Options.NAVIGATION_PAGES, new NavigationPageOptionItems());
        }

        //public OptionsRequest(HttpClient httpClient, TokenProvider? tokenProvider)
        //    : base(httpClient, tokenProvider)
        //{
        //    _localOptionItems.Add(Options.PAGE_CODES, new PageCodeOptionItems());
        //    _localOptionItems.Add(Options.NAVIGATION_PAGES, new NavigationPageOptionItems());
        //}

        public async Task<IResponse<IEnumerable<OptionItem>?>> GetOptionItems(IEnumerable<OptionsArg> optionsArgs)
        {
            var optionsCode = optionsArgs.FirstOptionsArgValue(Options.OPTIONS_CODE);

            if (!string.IsNullOrWhiteSpace(optionsCode)
                && _localOptionItems.ContainsKey(optionsCode))
            {
                return new Response<IEnumerable<OptionItem>?>
                {
                    IsSuccess = true,
                    Result = await _localOptionItems[optionsCode].GetOptionItemsAsync(optionsArgs).ConfigureAwait(false),
                };
            }

            using var httpResponseMessage = await _httpClient.PostAsJsonAsync(AtlasAPIEndpoints.GET_OPTIONS, optionsArgs)
                .ConfigureAwait(false);

            return await GetResponseAsync<IEnumerable<OptionItem>?>(httpResponseMessage)
                .ConfigureAwait(false);
        }

        public async Task<IResponse<IEnumerable<T>>> GetOptionItemsAsync<T>(IEnumerable<OptionsArg> optionsArgs)
        {
            using var httpResponseMessage = await _httpClient.PostAsJsonAsync(AtlasAPIEndpoints.GET_GENERIC_OPTIONS, optionsArgs)
                .ConfigureAwait(false);

            return await GetResponseAsync<IEnumerable<T>>(httpResponseMessage)
                .ConfigureAwait(false);
        }
    }
}

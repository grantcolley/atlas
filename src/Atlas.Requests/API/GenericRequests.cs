using Atlas.Core.Models;
using Atlas.Requests.Base;
using Atlas.Requests.Interfaces;
using Atlas.Requests.Model;

namespace Atlas.Requests.API
{
    public class GenericRequests : RequestBase, IGenericRequests
    {
        public GenericRequests(HttpClient httpClient)
            : base(httpClient)
        {
        }

        public GenericRequests(HttpClient httpClient, TokenProvider tokenProvider)
            : base(httpClient, tokenProvider)
        {
        }

        public async Task<IResponse<IEnumerable<T>>> GetGenericListAsync<T>(string endpoint) where T : class, new()
        {
            using var httpResponseMessage = await _httpClient.GetAsync(endpoint).ConfigureAwait(false);

            var responseList = await GetResponseAsync<IEnumerable<T>>(httpResponseMessage)
                .ConfigureAwait(false);

            Response<IEnumerable<T>> response = new()
            {
                IsSuccess = responseList.IsSuccess,
                Message = responseList.Message
            };

            if (responseList.IsSuccess)
            {
                response.Result = responseList.Result;
            }

            return response;
        }

        public async Task<IResponse<T>> GetGenericModelAsync<T>(int id, string endpoint) where T : class, new()
        {
            using var httpResponseMessage = await _httpClient.GetAsync($"{endpoint}/{id}").ConfigureAwait(false);

            var responseModel = await GetResponseAsync<T>(httpResponseMessage)
                .ConfigureAwait(false);

            Response<T> response = new()
            {
                IsSuccess = responseModel.IsSuccess,
                Message = responseModel.Message
            };

            if (responseModel.IsSuccess)
            {
                response.Result = responseModel.Result;
            }

            return response;
        }
    }
}

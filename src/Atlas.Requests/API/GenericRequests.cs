using Atlas.Core.Models;
using Atlas.Requests.Base;
using Atlas.Requests.Interfaces;
using Atlas.Requests.Model;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

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

        public async Task<IResponse<IEnumerable<T>>> GetListAsync<T>(string endpoint) where T : class, new()
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

        public async Task<IResponse<T>> GetModelAsync<T>(int id, string endpoint) where T : class, new()
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

        public async Task<IResponse<T>> CreateModelAsync<T>(T model, string endpoint) where T : class, new()
        {
            using var addResponse = await _httpClient.PostAsJsonAsync(endpoint, model)
                .ConfigureAwait(false);

            return await GetResponseAsync<T>(addResponse)
                .ConfigureAwait(false);
        }

        public async Task<IResponse<T>> UpdateModelAsync<T>(T model, string endpoint) where T : class, new()
        {
            using var updateResponse = await _httpClient.PutAsJsonAsync(endpoint, model)
                .ConfigureAwait(false);

            return await GetResponseAsync<T>(updateResponse)
                .ConfigureAwait(false);
        }

        public async Task<IResponse<int>> DeleteModelAsync(int id, string endpoint)
        {
            var apiEndpoint = $@"{endpoint}/{id}";
            using var httpResponseMessage = await _httpClient.DeleteAsync(apiEndpoint)
                .ConfigureAwait(false);

            return await GetResponseAsync<int>(httpResponseMessage)
                .ConfigureAwait(false);
        }

    }
}

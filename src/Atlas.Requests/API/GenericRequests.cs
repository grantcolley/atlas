using Atlas.Core.Models;
using Atlas.Requests.Base;
using Atlas.Requests.Interfaces;
using Atlas.Requests.Model;
using System.Net.Http.Json;

namespace Atlas.Requests.API
{
    public class GenericRequests(HttpClient httpClient) : RequestBase(httpClient), IGenericRequests
    {
        public async Task<IAuthResponse<IEnumerable<T>>> GetListAsync<T>(string args, string endpoint) where T : class, new()
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(args, nameof(args));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(endpoint, nameof(endpoint));

            using HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync(endpoint, args)
                .ConfigureAwait(false);

            IResponse<AuthResult<List<T>>> responseList = await GetResponseAsync<AuthResult<List<T>>>(httpResponseMessage)
                .ConfigureAwait(false);

            if (responseList == null) throw new NullReferenceException(nameof(responseList));

            AuthResponse<IEnumerable<T>> response = new()
            {
                IsSuccess = responseList.IsSuccess,
                Message = responseList.Message,
                Authorisation = responseList?.Result?.Authorisation,
                Result = responseList?.Result?.Result
            };

            return response;
        }

        public async Task<IAuthResponse<IEnumerable<T>>> GetListAsync<T>(string endpoint) where T : class, new()
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(endpoint, nameof(endpoint));

            using HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(endpoint).ConfigureAwait(false);

            IResponse<AuthResult<List<T>>> responseList = await GetResponseAsync<AuthResult<List<T>>>(httpResponseMessage)
                .ConfigureAwait(false);

            if (responseList == null) throw new NullReferenceException(nameof(responseList));

            AuthResponse<IEnumerable<T>> response = new()
            {
                IsSuccess = responseList.IsSuccess,
                Message = responseList.Message,
                Authorisation = responseList?.Result?.Authorisation,
                Result = responseList?.Result?.Result
            };

            return response;
        }

        public async Task<IResponse<T>> GetModelAsync<T>(int id, string endpoint) where T : class, new()
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(endpoint, nameof(endpoint));

            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"{endpoint}/{id}").ConfigureAwait(false);

            IResponse<T> responseModel = await GetResponseAsync<T>(httpResponseMessage)
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
            ArgumentNullException.ThrowIfNull(model, nameof(model));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(endpoint, nameof(endpoint));

            using HttpResponseMessage addResponse = await _httpClient.PostAsJsonAsync(endpoint, model, _jsonSerializerOptions)
                .ConfigureAwait(false);

            return await GetResponseAsync<T>(addResponse)
                .ConfigureAwait(false);
        }

        public async Task<IResponse<T>> UpdateModelAsync<T>(T model, string endpoint) where T : class, new()
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(endpoint, nameof(endpoint));

            using HttpResponseMessage updateResponse = await _httpClient.PutAsJsonAsync(endpoint, model, _jsonSerializerOptions)
                .ConfigureAwait(false);

            return await GetResponseAsync<T>(updateResponse)
                .ConfigureAwait(false);
        }

        public async Task<IResponse<int>> DeleteModelAsync(int id, string endpoint)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(endpoint, nameof(endpoint));

            string apiEndpoint = $@"{endpoint}/{id}";
            using HttpResponseMessage httpResponseMessage = await _httpClient.DeleteAsync(apiEndpoint)
                .ConfigureAwait(false);

            return await GetResponseAsync<int>(httpResponseMessage)
                .ConfigureAwait(false);
        }
    }
}

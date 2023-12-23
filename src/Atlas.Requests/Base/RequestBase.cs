using Atlas.Core.Authentication;
using Atlas.Requests.Interfaces;
using Atlas.Requests.Model;
using System.Text.Json;

namespace Atlas.Requests.Base
{
    public abstract class RequestBase
    {
        protected readonly HttpClient _httpClient;

        protected RequestBase(HttpClient httpClient) : this(httpClient, null)
        {
        }

        protected RequestBase(HttpClient httpClient, TokenProvider? tokenProvider)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            if (tokenProvider != null
                && !string.IsNullOrWhiteSpace(tokenProvider.AccessToken)
                && _httpClient.DefaultRequestHeaders.Authorization == null)
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenProvider.AccessToken}");
            }
        }

        protected static async Task<IResponse<T>> GetResponseAsync<T>(HttpResponseMessage httpResponseMessage)
        {
            var response = new Response<T>
            {
                IsSuccess = httpResponseMessage.IsSuccessStatusCode,
                Message = httpResponseMessage.ReasonPhrase
            };

            if (response.IsSuccess)
            {
                response.Result = await JsonSerializer.DeserializeAsync<T>
                    (await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false),
                        new JsonSerializerOptions(JsonSerializerDefaults.Web)).ConfigureAwait(false);
            }
            else
            {
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                string? debug = httpResponseMessage.Content.ReadAsStringAsync().Result;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
            }

            return response;
        }
    }
}

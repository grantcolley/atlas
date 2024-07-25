using Atlas.Requests.Interfaces;
using Atlas.Requests.Model;
using System.Text.Json;

namespace Atlas.Requests.Base
{
    public abstract class RequestBase(HttpClient httpClient)
    {
        protected static readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web);
        protected readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

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
                        _jsonSerializerOptions).ConfigureAwait(false);
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

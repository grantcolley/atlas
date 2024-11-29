using Atlas.Requests.Interfaces;
using Atlas.Requests.Model;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Atlas.Requests.Base
{
    public abstract class RequestBase(HttpClient httpClient)
    {
        protected static readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web) { ReferenceHandler = ReferenceHandler.IgnoreCycles };
        protected readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        protected static async Task<IResponse<T>> GetResponseAsync<T>(HttpResponseMessage httpResponseMessage)
        {
            Response<T> response = new()
            {
                IsSuccess = httpResponseMessage.IsSuccessStatusCode
            };

            if (response.IsSuccess)
            {
                response.Result = await JsonSerializer.DeserializeAsync<T>
                    (await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false),
                        _jsonSerializerOptions).ConfigureAwait(false);

                response.Message = httpResponseMessage.ReasonPhrase;
            }
            else
            {
                response.Message = $"{(int)httpResponseMessage.StatusCode} {httpResponseMessage.ReasonPhrase}";

#pragma warning disable IDE0059 // Unnecessary assignment of a value
                string? debug = httpResponseMessage.Content.ReadAsStringAsync().Result;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
            }

            return response;
        }
    }
}

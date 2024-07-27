using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Requests.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace Atlas.Requests.API
{
    public class LogRequests(HttpClient httpClient) : ILogRequests
    {
        protected readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        protected readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web);

        public async Task LogAsync(Log log)
        {
            using HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(AtlasAPIEndpoints.GET_OPTIONS, log)
                .ConfigureAwait(false);
        }
    }
}
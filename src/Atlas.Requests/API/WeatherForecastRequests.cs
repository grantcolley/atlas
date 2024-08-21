using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Requests.Interfaces;
using System.Text.Json;

namespace Atlas.Requests.API
{
    public class WeatherForecastRequests(HttpClient httpClient) : IWeatherForecastRequests
    {
        protected readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        protected readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web);

        public async Task<IEnumerable<WeatherForecast>?> GetWeatherForecastAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<WeatherForecast>?>
                (await _httpClient.GetStreamAsync(AtlasAPIEndpoints.GET_WEATHER_FORECAST)
                .ConfigureAwait(false), _jsonSerializerOptions).ConfigureAwait(false);
        }
    }
}

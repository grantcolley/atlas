using Atlas.Core.Authentication;
using Atlas.Core.Models;
using Atlas.Requests.Base;
using Atlas.Requests.Interfaces;
using System.Text.Json;

namespace Atlas.Requests.API
{
    public class WeatherForecastRequests : RequestBase, IWeatherForecastRequests
    {
        public WeatherForecastRequests(HttpClient httpClient)
            : base(httpClient)
        {
        }

        public WeatherForecastRequests(HttpClient httpClient, TokenProvider tokenProvider)
            : base(httpClient, tokenProvider)
        {
        }

        public async Task<IEnumerable<WeatherForecast>?> GetWeatherForecasts()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<WeatherForecast>?>
                (await _httpClient.GetStreamAsync($"weatherforecast")
                .ConfigureAwait(false),
                new JsonSerializerOptions(JsonSerializerDefaults.Web))
                .ConfigureAwait(false);
        }
    }
}

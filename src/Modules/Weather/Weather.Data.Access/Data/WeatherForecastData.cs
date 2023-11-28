using Atlas.Data.Access.Base;
using Atlas.Data.Access.Context;
using Microsoft.Extensions.Logging;
using Weather.Core.Model;

namespace Weather.Data.Access.Data
{
    public class WeatherForecastData : AuthorisationData<WeatherForecastData>, IWeatherForecastData
    {
        public WeatherForecastData(ApplicationDbContext applicationDbContext, ILogger<WeatherForecastData> logger)
            : base(applicationDbContext, logger)
        {
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild",
            "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecastsAsync(CancellationToken cancellationToken)
        {
            // Simulate asynchronous loading to demonstrate streaming rendering
            await Task.Delay(500);

            var startDate = DateOnly.FromDateTime(DateTime.Now);
            var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
            IEnumerable<WeatherForecast>? weatherForecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries[Random.Shared.Next(summaries.Length)]
            }).ToArray();

            return await Task.FromResult(weatherForecasts);
        }
    }
}

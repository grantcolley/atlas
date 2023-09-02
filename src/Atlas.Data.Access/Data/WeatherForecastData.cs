using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;

namespace Atlas.Data.Access.Data
{
    public class WeatherForecastData : IWeatherForecastData
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild",
            "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecasts(CancellationToken cancellationToken)
        {
            var rng = new Random();
            WeatherForecast[] weatherForecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            return await Task.FromResult(weatherForecasts);
        }
    }
}

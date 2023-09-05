using Atlas.Core.Models;

namespace Atlas.Data.Access.Interfaces
{
    public interface IWeatherForecastData
    {
        Task<IEnumerable<WeatherForecast>> GetWeatherForecastsAsync(CancellationToken cancellationToken);
    }
}

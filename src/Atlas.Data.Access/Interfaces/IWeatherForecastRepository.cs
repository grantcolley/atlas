using Atlas.Core.Models;

namespace Atlas.Data.Access.Interfaces
{
    public interface IWeatherForecastRepository
    {
        Task<IEnumerable<WeatherForecast>> GetWeatherForecasts(CancellationToken cancellationToken);
    }
}

using Atlas.Core.Models;

namespace Atlas.Data.Access.Interfaces
{
    public interface IWeatherForecastData : IAuthorisationData
    {
        Task<IEnumerable<WeatherForecast>> GetWeatherForecastsAsync(CancellationToken cancellationToken);
    }
}

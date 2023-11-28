using Atlas.Data.Access.Interfaces;
using Weather.Core.Model;

namespace Weather.Data.Access.Data
{
    public interface IWeatherForecastData : IAuthorisationData
    {
        Task<IEnumerable<WeatherForecast>> GetWeatherForecastsAsync(CancellationToken cancellationToken);
    }
}

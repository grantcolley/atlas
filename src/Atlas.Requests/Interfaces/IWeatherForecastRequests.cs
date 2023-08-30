using Atlas.Core.Models;

namespace Atlas.Requests.Interfaces
{
    public interface IWeatherForecastRequests
    {
        Task<IEnumerable<WeatherForecast>?> GetWeatherForecasts();
    }
}

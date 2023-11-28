using Atlas.Requests.Interfaces;
using Weather.Core.Model;

namespace Weather.Client.Requests
{
    public interface IWeatherForecastRequests
    {
        Task<IResponse<IEnumerable<WeatherForecast>>> GetWeatherForecastsAsync();
    }
}

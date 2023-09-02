using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;

namespace Atlas.API.Endpoints
{
    internal static class WeatherForecastEndpoint
    {
        internal static async Task<IResult> GetWeatherForecast(IWeatherForecastData weatherForecastRepository, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<WeatherForecast>? weatherForecasts = await weatherForecastRepository.GetWeatherForecasts(cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(weatherForecasts);
            }
            catch (Exception)
            {
                // Exceptions thrown from weatherForecastRepository.GetWeatherForecasts(token)
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

using Atlas.API.Interfaces;
using Atlas.Core.Constants;
using Atlas.Core.Models;
using Weather.Core.Model;
using Weather.Data.Access.Data;

namespace Atlas.API.Endpoints
{
    internal static class WeatherForecastEndpoint
    {
        internal static async Task<IResult> GetWeatherForecast(IWeatherForecastData weatherForecastData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await weatherForecastData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.WEATHER_USER))
                {
                    return Results.Unauthorized();
                }

                IEnumerable<WeatherForecast>? weatherForecasts = await weatherForecastData.GetWeatherForecastsAsync(cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(weatherForecasts);
            }
            catch (Exception)
            {
                // Exceptions thrown from weatherForecastData.GetWeatherForecastsAsync(token)
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

﻿using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;

namespace Atlas.API.Endpoints
{
    internal static class WeatherForecastEndpoint
    {
        internal static async Task<IResult> GetWeatherForecast(IWeatherForecastData weatherForecastData, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<WeatherForecast>? weatherForecasts = await weatherForecastData.GetWeatherForecastsAsync(cancellationToken)
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

using Atlas.API.Endpoints;
using Atlas.Core.Constants;
using Atlas.Core.Models;

namespace Atlas.API.Extensions
{
    internal static class ModulesEndpointMapper
    {
        internal static WebApplication? MapAtlasModulesEndpoints(this WebApplication app)
        {
            // Additional module API's mapped here...

            app.MapGet($"/{AtlasAPIEndpoints.GET_WEATHER_FORECAST}", WeatherEndpoints.GetWeatherForecast)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_WEATHER_FORECAST)
                .WithDescription("Gets the weather forecast")
                .Produces<IEnumerable<WeatherForecast>?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            return app;
        }
    }
}

using Atlas.API.Endpoints;
using Atlas.Core.Constants;
using Weather.Core.Model;

namespace Atlas.API.Extensions
{
    public static class ModulesEndpointMapper
    {
        public static WebApplication? MapAtlasModulesEndpoints(this WebApplication app)
        {
            app.MapGet("/weatherforecast", WeatherForecastEndpoint.GetWeatherForecast)
                .WithOpenApi()
                .WithName("weatherorecast")
                .WithDescription("Gets the weather forecast")
                .Produces<IEnumerable<WeatherForecast>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            return app;
        }
    }
}

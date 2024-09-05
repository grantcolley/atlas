using Atlas.API.Endpoints;
using Atlas.Core.Constants;
using Atlas.Core.Models;

namespace Atlas.API.Extensions
{
    internal static class SupportEndpointMapper
    {
        internal static WebApplication? MapAtlasSupportEndpoints(this WebApplication app)
        {
            app.MapPost($"/{AtlasAPIEndpoints.GET_LOGS}", SupportEndpoints.GetLogs)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_LOGS)
                .WithDescription("Gets a list of logs")
                .Produces<IEnumerable<Log>?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            return app;
        }
    }
}

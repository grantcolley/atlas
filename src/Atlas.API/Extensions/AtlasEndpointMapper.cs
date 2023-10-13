using Atlas.API.Endpoints;
using Atlas.Core.Constants;
using Atlas.Core.Models;

namespace Atlas.API.Extensions
{
    public static class AtlasEndpointMapper
    {
        public static WebApplication? MapAtlas(this WebApplication app)
        {
            app.MapGet($"/{AtlasAPIEndpoints.GET_CLAIM_MODULES}", UserEndpoint.GetClaimModules)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_CLAIM_MODULES)
                .WithDescription("Gets the user's authorized modules")
                .Produces<IEnumerable<Module>?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapGet($"/{AtlasAPIEndpoints.GET_THEME}", UserEndpoint.GetTheme)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_THEME)
                .WithDescription("Gets the user's theme preference")
                .Produces<IEnumerable<Module>?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapPost($"/{AtlasAPIEndpoints.SET_THEME}", UserEndpoint.SetTheme)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.SET_THEME)
                .WithDescription("Sets the user's theme preference")
                .Produces<IEnumerable<Module>?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            return app;
        }
    }
}

using Atlas.API.Endpoints;
using Atlas.Core.Constants;
using Atlas.Core.Models;

namespace Atlas.API.Extensions
{
    public static class AtlasEndpointMapper
    {
        public static WebApplication? MapAtlas(this WebApplication app)
        {
            app.MapGet($"/{AtlasAPIEndpoints.CLAIM_MODULES}", NavigationEndpoint.GetClaimModules)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.CLAIM_MODULES)
                .WithDescription("Gets the user's authorized modules")
                .Produces<IEnumerable<Module>?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapGet($"/{AtlasAPIEndpoints.COMPONENT_ARGS}/{{componentCode}}", ComponentArgsEndpoint.GetComponentArgs)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.COMPONENT_ARGS)
                .WithDescription("Gets args associated with a component to be rendered in the browser")
                .Produces<ComponentArgs?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            return app;
        }
    }
}

using Atlas.API.Endpoints;
using Atlas.Core.Constants;
using Atlas.Core.Models;

namespace Atlas.API.Extensions
{
    public static class AtlasEndpointMapper
    {
        public static WebApplication? MapAtlasEndpoints(this WebApplication app)
        {
            app.MapGet($"/{AtlasAPIEndpoints.GET_CLAIM_AUTHORIZATION}", ClaimEndpoint.GetClaimAuthorisation)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_CLAIM_AUTHORIZATION)
                .WithDescription("Gets the user's authorization")
                .Produces<IEnumerable<Module>?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapGet($"/{AtlasAPIEndpoints.GET_CLAIM_MODULES}", ClaimEndpoint.GetClaimModules)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_CLAIM_MODULES)
                .WithDescription("Gets the user's authorized modules")
                .Produces<IEnumerable<Module>?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapPost($"/{AtlasAPIEndpoints.GET_OPTIONS}", OptionsEndpoints.GetOptions)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_OPTIONS)
                .WithDescription("Gets option items for the specified options code")
                .Produces<IEnumerable<OptionItem>?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapPost($"/{AtlasAPIEndpoints.GET_GENERIC_OPTIONS}", OptionsEndpoints.GetGenericOptions)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_GENERIC_OPTIONS)
                .WithDescription("Gets generic options for the specified options code")
                .Produces<string>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            return app;
        }
    }
}

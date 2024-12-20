using Atlas.API.Endpoints;
using Atlas.Core.Constants;
using Atlas.Core.Models;

namespace Atlas.API.Extensions
{
    internal static class AtlasDatabaseEndpointMapper
    {
        internal static WebApplication? MapAtlasDatabaseEndpoints(this WebApplication app)
        {
            app.MapGet($"/{AtlasAPIEndpoints.GET_DATABASE_STATUS}", DeveloperEndpoints.GetDatabaseStatus)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_DATABASE_STATUS)
                .WithDescription("Gets the database status")
                .Produces<DatabaseStatus?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_DEVELOPER_CLAIM);

            app.MapGet($"/{AtlasAPIEndpoints.CREATE_DATABASE}", DeveloperEndpoints.CreateDatabase)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.CREATE_DATABASE)
                .WithDescription("Create the database")
                .Produces<DatabaseStatus?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_DEVELOPER_CLAIM);

            app.MapGet($"/{AtlasAPIEndpoints.SEED_DATABASE}", DeveloperEndpoints.SeedDatabase)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.SEED_DATABASE)
                .WithDescription("Seed the database")
                .Produces<DatabaseStatus?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_DEVELOPER_CLAIM);

            return app;
        }
    }
}

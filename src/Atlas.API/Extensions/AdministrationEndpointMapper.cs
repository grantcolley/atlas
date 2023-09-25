using Atlas.API.Endpoints;
using Atlas.Core.Constants;
using Atlas.Core.Models;

namespace Atlas.API.Extensions
{
    public static class AdministrationEndpointMapper
    {
        public static WebApplication? MapAtlasAdministration(this WebApplication app)
        {
            app.MapGet($"/{AtlasAPIEndpoints.GET_MODULES}", AdministrationEndpoints.GetModules)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_MODULES)
                .WithDescription("Gets a list of modules")
                .Produces<IEnumerable<Module>?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapGet($"/{AtlasAPIEndpoints.GET_MODULE}/{{id:int}}", AdministrationEndpoints.GetModule)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_MODULE)
                .WithDescription("Gets a module for the given id. If id is 0 then returns a new instance of a blank module for creation.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapPost($"/{AtlasAPIEndpoints.CREATE_MODULE}", AdministrationEndpoints.CreateModule)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.CREATE_MODULE)
                .WithDescription("Create a new module.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapPut($"/{AtlasAPIEndpoints.UPDATE_MODULE}", AdministrationEndpoints.UpdateModule)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.UPDATE_MODULE)
                .WithDescription("Updates the module.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapDelete($"/{AtlasAPIEndpoints.DELETE_MODULE}/{{id:int}}", AdministrationEndpoints.DeleteModule)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.DELETE_MODULE)
                .WithDescription("Delete's a module of the given id.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            return app;
        }
    }
}

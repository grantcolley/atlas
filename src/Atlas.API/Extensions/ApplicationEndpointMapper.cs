using Atlas.API.Endpoints;
using Atlas.Core.Constants;
using Atlas.Core.Models;

namespace Atlas.API.Extensions
{
    internal static class ApplicationEndpointMapper
    {
        internal static WebApplication? MapAtlasApplicationEndpoints(this WebApplication app)
        {
            app.MapGet($"/{AtlasAPIEndpoints.GET_MODULES}", ApplicationEndpoints.GetModules)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_MODULES)
                .WithDescription("Gets a list of modules")
                .Produces<IEnumerable<Module>?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapGet($"/{AtlasAPIEndpoints.GET_MODULE}/{{id:int}}", ApplicationEndpoints.GetModule)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_MODULE)
                .WithDescription("Gets a module for the given id. If id is 0 then returns a new instance of a blank module for creation.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapPost($"/{AtlasAPIEndpoints.CREATE_MODULE}", ApplicationEndpoints.CreateModule)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.CREATE_MODULE)
                .WithDescription("Create a new module.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapPut($"/{AtlasAPIEndpoints.UPDATE_MODULE}", ApplicationEndpoints.UpdateModule)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.UPDATE_MODULE)
                .WithDescription("Updates the module.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapDelete($"/{AtlasAPIEndpoints.DELETE_MODULE}/{{id:int}}", ApplicationEndpoints.DeleteModule)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.DELETE_MODULE)
                .WithDescription("Delete's a module of the given id.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapGet($"/{AtlasAPIEndpoints.GET_CATEGORIES}", ApplicationEndpoints.GetCategories)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_CATEGORIES)
                .WithDescription("Gets a list of categories")
                .Produces<IEnumerable<Module>?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapGet($"/{AtlasAPIEndpoints.GET_CATEGORY}/{{id:int}}", ApplicationEndpoints.GetCategory)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_CATEGORY)
                .WithDescription("Gets a category for the given id. If id is 0 then returns a new instance of a blank category for creation.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapPost($"/{AtlasAPIEndpoints.CREATE_CATEGORY}", ApplicationEndpoints.CreateCategory)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.CREATE_CATEGORY)
                .WithDescription("Create a new category.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapPut($"/{AtlasAPIEndpoints.UPDATE_CATEGORY}", ApplicationEndpoints.UpdateCategory)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.UPDATE_CATEGORY)
                .WithDescription("Updates the category.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapDelete($"/{AtlasAPIEndpoints.DELETE_CATEGORY}/{{id:int}}", ApplicationEndpoints.DeleteCategory)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.DELETE_CATEGORY)
                .WithDescription("Delete's a category of the given id.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapGet($"/{AtlasAPIEndpoints.GET_PAGES}", ApplicationEndpoints.GetPages)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_PAGES)
                .WithDescription("Gets a list of pages")
                .Produces<IEnumerable<Module>?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapGet($"/{AtlasAPIEndpoints.GET_PAGE}/{{id:int}}", ApplicationEndpoints.GetPage)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_PAGE)
                .WithDescription("Gets a page for the given id. If id is 0 then returns a new instance of a blank page for creation.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapPost($"/{AtlasAPIEndpoints.CREATE_PAGE}", ApplicationEndpoints.CreatePage)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.CREATE_PAGE)
                .WithDescription("Create a new page.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapPut($"/{AtlasAPIEndpoints.UPDATE_PAGE}", ApplicationEndpoints.UpdatePage)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.UPDATE_PAGE)
                .WithDescription("Updates the page.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapDelete($"/{AtlasAPIEndpoints.DELETE_PAGE}/{{id:int}}", ApplicationEndpoints.DeletePage)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.DELETE_PAGE)
                .WithDescription("Delete's a page of the given id.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            return app;
        }
    }
}

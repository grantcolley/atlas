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

            app.MapGet($"/{AtlasAPIEndpoints.GET_CATEGORIES}", AdministrationEndpoints.GetCategories)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_CATEGORIES)
                .WithDescription("Gets a list of categories")
                .Produces<IEnumerable<Module>?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapGet($"/{AtlasAPIEndpoints.GET_CATEGORY}/{{id:int}}", AdministrationEndpoints.GetCategory)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_CATEGORY)
                .WithDescription("Gets a category for the given id. If id is 0 then returns a new instance of a blank category for creation.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapPost($"/{AtlasAPIEndpoints.CREATE_CATEGORY}", AdministrationEndpoints.CreateCategory)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.CREATE_CATEGORY)
                .WithDescription("Create a new category.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapPut($"/{AtlasAPIEndpoints.UPDATE_CATEGORY}", AdministrationEndpoints.UpdateCategory)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.UPDATE_CATEGORY)
                .WithDescription("Updates the category.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapDelete($"/{AtlasAPIEndpoints.DELETE_CATEGORY}/{{id:int}}", AdministrationEndpoints.DeleteCategory)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.DELETE_CATEGORY)
                .WithDescription("Delete's a category of the given id.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapGet($"/{AtlasAPIEndpoints.GET_MENU_ITEMS}", AdministrationEndpoints.GetMenuItems)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_MENU_ITEMS)
                .WithDescription("Gets a list of menu items")
                .Produces<IEnumerable<Module>?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapGet($"/{AtlasAPIEndpoints.GET_MENU_ITEM}/{{id:int}}", AdministrationEndpoints.GetMenuItem)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_MENU_ITEM)
                .WithDescription("Gets a menu item for the given id. If id is 0 then returns a new instance of a blank menu item for creation.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapPost($"/{AtlasAPIEndpoints.CREATE_MENU_ITEM}", AdministrationEndpoints.CreateMenuItem)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.CREATE_MENU_ITEM)
                .WithDescription("Create a new menu item.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapPut($"/{AtlasAPIEndpoints.UPDATE_MENU_ITEM}", AdministrationEndpoints.UpdateMenuItem)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.UPDATE_MENU_ITEM)
                .WithDescription("Updates the menu item.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapDelete($"/{AtlasAPIEndpoints.DELETE_MENU_ITEM}/{{id:int}}", AdministrationEndpoints.DeleteMenuItem)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.DELETE_MENU_ITEM)
                .WithDescription("Delete's a menu item of the given id.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            return app;
        }
    }
}

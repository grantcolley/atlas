using Atlas.API.Endpoints;
using Atlas.Core.Constants;
using Atlas.Core.Models;

namespace Atlas.API.Extensions
{
    internal static class AdministrationEndpointMapper
    {
        internal static WebApplication? MapAtlasAdministrationEndpoints(this WebApplication app)
        {
            app.MapGet($"/{AtlasAPIEndpoints.GET_USERS}", AdministrationEndpoints.GetUsers)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_USERS)
                .WithDescription("Gets a list of users")
                .Produces<IEnumerable<Module>?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapGet($"/{AtlasAPIEndpoints.GET_USER}/{{id:int}}", AdministrationEndpoints.GetUser)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_USER)
                .WithDescription("Gets a user for the given id. If id is 0 then returns a new instance of a blank user for creation.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapPost($"/{AtlasAPIEndpoints.CREATE_USER}", AdministrationEndpoints.CreateUser)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.CREATE_USER)
                .WithDescription("Create a new user.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapPut($"/{AtlasAPIEndpoints.UPDATE_USER}", AdministrationEndpoints.UpdateUser)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.UPDATE_USER)
                .WithDescription("Updates the user.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapDelete($"/{AtlasAPIEndpoints.DELETE_USER}/{{id:int}}", AdministrationEndpoints.DeleteUser)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.DELETE_USER)
                .WithDescription("Delete's a user of the given id.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapGet($"/{AtlasAPIEndpoints.GET_ROLES}", AdministrationEndpoints.GetRoles)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_ROLES)
                .WithDescription("Gets a list of roles")
                .Produces<IEnumerable<Module>?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapGet($"/{AtlasAPIEndpoints.GET_ROLE}/{{id:int}}", AdministrationEndpoints.GetRole)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_ROLE)
                .WithDescription("Gets a role for the given id. If id is 0 then returns a new instance of a blank role for creation.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapPost($"/{AtlasAPIEndpoints.CREATE_ROLE}", AdministrationEndpoints.CreateRole)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.CREATE_ROLE)
                .WithDescription("Create a new role.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapPut($"/{AtlasAPIEndpoints.UPDATE_ROLE}", AdministrationEndpoints.UpdateRole)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.UPDATE_ROLE)
                .WithDescription("Updates the role.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapDelete($"/{AtlasAPIEndpoints.DELETE_ROLE}/{{id:int}}", AdministrationEndpoints.DeleteRole)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.DELETE_ROLE)
                .WithDescription("Delete's a role of the given id.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapGet($"/{AtlasAPIEndpoints.GET_PERMISSIONS}", AdministrationEndpoints.GetPermissions)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_PERMISSIONS)
                .WithDescription("Gets a list of permissions")
                .Produces<IEnumerable<Module>?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapGet($"/{AtlasAPIEndpoints.GET_PERMISSION}/{{id:int}}", AdministrationEndpoints.GetPermission)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_PERMISSION)
                .WithDescription("Gets a permission for the given id. If id is 0 then returns a new instance of a blank permission for creation.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapPost($"/{AtlasAPIEndpoints.CREATE_PERMISSION}", AdministrationEndpoints.CreatePermission)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.CREATE_PERMISSION)
                .WithDescription("Create a new permission.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapPut($"/{AtlasAPIEndpoints.UPDATE_PERMISSION}", AdministrationEndpoints.UpdatePermission)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.UPDATE_PERMISSION)
                .WithDescription("Updates the permission.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapDelete($"/{AtlasAPIEndpoints.DELETE_PERMISSION}/{{id:int}}", AdministrationEndpoints.DeletePermission)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.DELETE_PERMISSION)
                .WithDescription("Delete's a permission of the given id.")
                .Produces<Module>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapGet($"/{AtlasAPIEndpoints.GET_PERMISSION_CHECKLIST}", AdministrationEndpoints.GetPermissionChecklistAsync)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_PERMISSION_CHECKLIST)
                .WithDescription("Gets a checklist of permissions")
                .Produces<IEnumerable<Module>?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            app.MapGet($"/{AtlasAPIEndpoints.GET_ROLE_CHECKLIST}", AdministrationEndpoints.GetRoleChecklistAsync)
                .WithOpenApi()
                .WithName(AtlasAPIEndpoints.GET_ROLE_CHECKLIST)
                .WithDescription("Gets a checklist of roles")
                .Produces<IEnumerable<Module>?>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequireAuthorization(Auth.ATLAS_USER_CLAIM);

            return app;
        }
    }
}

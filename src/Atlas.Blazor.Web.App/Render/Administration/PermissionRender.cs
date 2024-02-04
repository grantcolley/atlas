using Atlas.Blazor.Web.App.Constants;
using Atlas.Core.Models;

namespace Atlas.Blazor.Web.App.Render.Administration
{
    public class PermissionRender : ModelRender<Permission>
    {
        public override void Configure()
        {
            string permissionContainerCode = "PERMISSION";
            string rolesContainerCode = "ROLES";
            string usersContainerCode = "USERS";

            RenderContainer(new Container { ContainerCode = permissionContainerCode, Title = "Permission", Order = 1 });
            RenderContainer(new Container { ContainerCode = rolesContainerCode, Title = "Roles", Order = 2 });
            RenderContainer(new Container { ContainerCode = usersContainerCode, Title = "Users", Order = 3 });

            RenderProperty(p => p.PermissionId)
                .RenderAs(Element.Label)
                .InContainer(permissionContainerCode)
                .WithLabel("Role Id")
                .WithTooltip("Role Id")
                .RenderOrder(1);

            RenderProperty(p => p.Name)
                .RenderAs(Element.Text)
                .InContainer(permissionContainerCode)
                .WithLabel("Name")
                .WithTooltip("Name")
                .RenderOrder(2);

            RenderProperty(p => p.Description)
                .RenderAs(Element.Text)
                .InContainer(permissionContainerCode)
                .WithLabel("Description")
                .WithTooltip("Description")
                .RenderOrder(3);

            RenderProperty(p => p.RoleList)
                .RenderAs(Element.ReadList)
                .InContainer(rolesContainerCode)
                .WithLabel("Roles")
                .WithTooltip("Roles")
                .RenderOrder(1);

            RenderProperty(p => p.UserList)
                .RenderAs(Element.ReadList)
                .InContainer(usersContainerCode)
                .WithLabel("Users")
                .WithTooltip("Users")
                .RenderOrder(1);
        }
    }
}

using Atlas.Blazor.Web.App.Constants;
using Atlas.Core.Models;

namespace Atlas.Blazor.Web.App.Render.Administration
{
    public class RoleRender : ModelRender<Role>
    {
        public override void Configure()
        {
            string roleContainerCode = "ROLE";
            string permissionsContainerCode = "PERMISSIONS";
            string usersContainerCode = "USERS";

            RenderContainer(new Container { ContainerCode = roleContainerCode, Title = "Role", Order = 1 });
            RenderContainer(new Container { ContainerCode = permissionsContainerCode, Title = "Permissions", Order = 2 });
            RenderContainer(new Container { ContainerCode = usersContainerCode, Title = "Users", Order = 3 });

            RenderProperty(r => r.RoleId)
                .RenderAs(Element.Label)
                .InContainer(roleContainerCode)
                .WithLabel("Role Id")
                .WithTooltip("Role Id")
                .RenderOrder(1);

            RenderProperty(r => r.Name)
                .RenderAs(Element.Text)
                .InContainer(roleContainerCode)
                .WithLabel("Name")
                .WithTooltip("Name")
                .RenderOrder(2);

            RenderProperty(r => r.Description)
                .RenderAs(Element.Text)
                .InContainer(roleContainerCode)
                .WithLabel("Description")
                .WithTooltip("Description")
                .RenderOrder(3);

            RenderProperty(r => r.PermissionChecklist)
                .RenderAs(Element.Checklist)
                .InContainer(permissionsContainerCode)
                .WithLabel("Permissions")
                .WithTooltip("Permissions")
                .RenderOrder(1);

            RenderProperty(r => r.UserList)
                .RenderAs(Element.ReadList)
                .InContainer(usersContainerCode)
                .WithLabel("Users")
                .WithTooltip("Users")
                .RenderOrder(1);
        }
    }
}

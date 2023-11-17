using Atlas.Blazor.UI.Constants;
using Atlas.Core.Models;

namespace Atlas.Blazor.UI.Render.Instances
{
    public class UserRender : ModelRender<User>
    {
        public override void Configure()
        {
            string userContainerCode = "USER";
            string rolesContainerCode = "ROLES";
            string permissionsContainerCode = "PERMISSIONS";

            RenderContainer(new Container { ContainerCode = userContainerCode, Title = "User", Order = 1 });
            RenderContainer(new Container { ContainerCode = rolesContainerCode, Title = "Roles", Order = 2 });
            RenderContainer(new Container { ContainerCode = permissionsContainerCode, Title = "Permissions", Order = 3 });

            RenderProperty(u => u.UserId)
                .RenderAs(Element.Label)
                .InContainer(userContainerCode)
                .WithLabel("User Id")
                .WithTooltip("User Id")
                .RenderOrder(1);

            RenderProperty(u => u.UserName)
                .RenderAs(Element.Text)
                .InContainer(userContainerCode)
                .WithLabel("User Name")
                .WithTooltip("User Name")
                .RenderOrder(2);

            RenderProperty(u => u.Email)
                .RenderAs(Element.Text)
                .InContainer(userContainerCode)
                .WithLabel("Email")
                .WithTooltip("Email")
                .RenderOrder(3);

            RenderProperty(u => u.Theme)
                .RenderAs(Element.DropdownIcon)
                .InContainer(userContainerCode)
                .WithLabel("Theme")
                .WithTooltip("Theme")
                .WithParameters(new Dictionary<string, string> { { IconsOptions.ICONS_CODE, IconsOptions.THEME } })
                .RenderOrder(4);

            RenderProperty(u => u.RoleChecklist)
                .RenderAs(Element.Checklist)
                .InContainer(rolesContainerCode)
                .WithLabel("Roles")
                .WithTooltip("Roles")
                .RenderOrder(1);

            RenderProperty(u => u.PermissionChecklist)
                .RenderAs(Element.Checklist)
                .InContainer(permissionsContainerCode)
                .WithLabel("Permissions")
                .WithTooltip("Permissions")
                .RenderOrder(1);
        }
    }
}
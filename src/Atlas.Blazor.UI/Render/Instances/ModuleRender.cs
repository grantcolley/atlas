using Atlas.Blazor.UI.Constants;
using Atlas.Core.Constants;
using Atlas.Core.Models;

namespace Atlas.Blazor.UI.Render.Instances
{
    public class ModuleRender : ModelRender<Module>
    {
        public override void Configure()
        {
            RenderProperty(m => m.ModuleId)
                .RenderAs(Element.Label)
                .WithLabel("Module Id")
                .WithTooltip("Module Id")
                .RenderOrder(1);

            RenderProperty(m => m.Name)
                .RenderAs(Element.Text)
                .WithLabel("Name")
                .WithTooltip("Name")
                .RenderOrder(2);

            RenderProperty(m => m.Order)
                .RenderAs(Element.Integer)
                .WithLabel("Order")
                .WithTooltip("Order")
                .RenderOrder(3);

            RenderProperty(m => m.Icon)
                .RenderAs(Element.DropdownIcon)
                .WithLabel("Icon")
                .WithTooltip("Icon")
                .WithParameters(new Dictionary<string, string> { { IconsOptions.ICONS_CODE, IconsOptions.ALL } })
                .RenderOrder(4);

            RenderProperty(m => m.Permission)
                .RenderAs(Element.Dropdown)
                .WithLabel("Permission")
                .WithTooltip("Permission")
                .WithParameters(new Dictionary<string, string> { { Options.OPTIONS_CODE, Options.PERMISSIONS_OPTION_ITEMS } })
                .RenderOrder(5);
        }
    }
}

using Atlas.Blazor.Shared.Constants;
using Atlas.Core.Models;

namespace Atlas.Blazor.Shared.Render.Instances
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

            RenderProperty(m => m.Icon)
                .RenderAs(Element.Text)
                .WithLabel("Icon")
                .WithTooltip("Icon")
                .RenderOrder(3);

            RenderProperty(m => m.Permission)
                .RenderAs(Element.Text)
                .WithLabel("Permission")
                .WithTooltip("Permission")
                .RenderOrder(4);
        }
    }
}

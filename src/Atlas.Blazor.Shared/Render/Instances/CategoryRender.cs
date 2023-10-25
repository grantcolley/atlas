using Atlas.Blazor.Shared.Constants;
using Atlas.Core.Constants;
using Atlas.Core.Models;

namespace Atlas.Blazor.Shared.Render.Instances
{
    public class CategoryRender : ModelRender<Category>
    {
        public override void Configure()
        {
            RenderProperty(m => m.CategoryId)
                .RenderAs(Element.Label)
                .WithLabel("Category Id")
                .WithTooltip("Category Id")
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
                .RenderOrder(4);

            RenderProperty(m => m.Module)
                .RenderAsGenericComponent(Element.DropdownGeneric)
                .WithLabel("Module")
                .WithTooltip("Module")
                .RenderOrder(5);

            RenderProperty(m => m.Permission)
                .RenderAs(Element.Dropdown)
                .WithLabel("Permission")
                .WithTooltip("Permission")
                .WithParameters(new Dictionary<string, string> { { Options.OPTIONS_CODE, Options.PERMISSIONS_OPTION_ITEMS } })
                .RenderOrder(6);
        }
    }
}

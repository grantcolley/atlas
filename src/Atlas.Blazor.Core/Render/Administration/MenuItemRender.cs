using Atlas.Blazor.Core.Constants;
using Atlas.Core.Constants;
using Atlas.Core.Models;
using System.Collections.Generic;

namespace Atlas.Blazor.Core.Render.Administration
{
    public class MenuItemRender : ModelRender<MenuItem>
    {
        public override void Configure()
        {
            RenderProperty(m => m.MenuItemId)
                .RenderAs(Element.Label)
                .WithLabel("Menu Item Id")
                .WithTooltip("Menu Item Id")
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

            RenderGenericProperty(m => m.Category)
                .RenderAsDropdownGeneric("Name")
                .WithLabel("Category")
                .WithTooltip("Category")
                .WithParameters(new Dictionary<string, string> { { Options.OPTIONS_CODE, Options.CATEGORIES_OPTION_ITEMS } })
                .RenderOrder(5);

            RenderProperty(c => c.Permission)
                .RenderAs(Element.Dropdown)
                .WithLabel("Permission")
                .WithTooltip("Permission")
                .WithParameters(new Dictionary<string, string> { { Options.OPTIONS_CODE, Options.PERMISSIONS_OPTION_ITEMS } })
                .RenderOrder(6);

            RenderProperty(m => m.PageCode)
                .RenderAs(Element.Dropdown)
                .WithLabel("Page Code")
                .WithTooltip("Page Code")
                .WithParameters(new Dictionary<string, string> { { Options.OPTIONS_CODE, Options.PAGE_CODES } })
                .RenderOrder(7);

            RenderProperty(m => m.NavigatePage)
                .RenderAs(Element.Dropdown)
                .WithLabel("Navigate Page")
                .WithTooltip("Navigate Page")
                .WithParameters(new Dictionary<string, string> { { Options.OPTIONS_CODE, Options.NAVIGATION_PAGES } })
                .RenderOrder(8);
        }
    }
}

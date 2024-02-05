﻿using Atlas.Blazor.Web.App.Constants;
using Atlas.Core.Constants;
using Atlas.Core.Models;
using System.Collections.Generic;

namespace Atlas.Blazor.Web.App.Render.Administration
{
    public class CategoryRender : ModelRender<Category>
    {
        public override void Configure()
        {
            RenderProperty(c => c.CategoryId)
                .RenderAs(Element.Label)
                .WithLabel("Category Id")
                .WithTooltip("Category Id")
                .RenderOrder(1);

            RenderProperty(c => c.Name)
                .RenderAs(Element.Text)
                .WithLabel("Name")
                .WithTooltip("Name")
                .RenderOrder(2);

            RenderProperty(c => c.Order)
                .RenderAs(Element.Integer)
                .WithLabel("Order")
                .WithTooltip("Order")
                .RenderOrder(3);

            RenderProperty(c => c.Icon)
                .RenderAs(Element.DropdownIcon)
                .WithLabel("Icon")
                .WithTooltip("Icon")
                .WithParameters(new Dictionary<string, string> { { IconsOptions.ICONS_CODE, IconsOptions.ALL } })
                .RenderOrder(4);

            RenderGenericProperty(c => c.Module)
                .RenderAsDropdownGeneric("Name")
                .WithLabel("Module")
                .WithTooltip("Module")
                .WithParameters(new Dictionary<string, string> { { Options.OPTIONS_CODE, Options.MODULES_OPTION_ITEMS } })
                .RenderOrder(5);

            RenderProperty(c => c.Permission)
                .RenderAs(Element.Dropdown)
                .WithLabel("Permission")
                .WithTooltip("Permission")
                .WithParameters(new Dictionary<string, string> { { Options.OPTIONS_CODE, Options.PERMISSIONS_OPTION_ITEMS } })
                .RenderOrder(6);
        }
    }
}
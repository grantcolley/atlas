using Atlas.Blazor.UI.Constants;
using Atlas.Blazor.UI.Models;

namespace Atlas.Blazor.UI.Helpers
{
    public static class PageRouterHelper
    {
        public static IEnumerable<PageArgs> GetPageArgs()
        {
            List<PageArgs> pageArgs = new()
            {
                    new PageArgs
                    {
                        PageCode = PageArgsCodes.MODULES,
                        ComponentName = "Atlas.Blazor.UI.Components.Admin.ModulesView, Atlas.Blazor.UI",
                        DisplayName = "Modules",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Module",
                        ModelParameters = new()
                        {
                            {
                                "Fields", new List<string> { "ModuleId", "Name", "Permission" }
                            },
                            {
                                "IdentifierField", "ModuleId"
                            }
                        }
                    },
                    new PageArgs
                    {
                        PageCode = PageArgsCodes.MODULE,
                        ComponentName = "Atlas.Blazor.UI.Components.Admin.ModuleView, Atlas.Blazor.UI",
                        DisplayName = "Category",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Module"
                    },
                    new PageArgs
                    {
                        PageCode = PageArgsCodes.CATEGORIES,
                        ComponentName = "Atlas.Blazor.UI.Components.Admin.CategoriesView, Atlas.Blazor.UI",
                        DisplayName = "Categories",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Category",
                        ModelParameters = new()
                        {
                            {
                                "Fields", new List<string> { "CategoryId", "Name", "Permission" }
                            },
                            {
                                "IdentifierField", "CategoryId"
                            }
                        }
                    },
                    new PageArgs
                    {
                        PageCode = PageArgsCodes.CATEGORY,
                        ComponentName = "Atlas.Blazor.UI.Components.Admin.CategoryView, Atlas.Blazor.UI",
                        DisplayName = "Category",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Category"
                    }
            };

            return pageArgs;
        }
    }
}
using Atlas.Blazor.Shared.Constants;
using Atlas.Blazor.Shared.Models;

namespace Atlas.Blazor.Shared.Helpers
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
                        ComponentName = "Atlas.Blazor.Shared.Components.Admin.ModulesView, Atlas.Blazor.Shared",
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
                        ComponentName = "Atlas.Blazor.Shared.Components.Admin.ModuleView, Atlas.Blazor.Shared",
                        DisplayName = "Category",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Module"
                    },
                    new PageArgs
                    {
                        PageCode = PageArgsCodes.CATEGORIES,
                        ComponentName = "Atlas.Blazor.Shared.Components.Admin.CategoriesView, Atlas.Blazor.Shared",
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
                        ComponentName = "Atlas.Blazor.Shared.Components.Admin.CategoryView, Atlas.Blazor.Shared",
                        DisplayName = "Category",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Category"
                    }
            };

            return pageArgs;
        }
    }
}
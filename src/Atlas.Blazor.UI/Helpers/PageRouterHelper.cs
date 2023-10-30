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
                        ComponentName = "Atlas.Blazor.UI.Components.Administration.ModulesView, Atlas.Blazor.UI",
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
                        ComponentName = "Atlas.Blazor.UI.Components.Administration.ModuleView, Atlas.Blazor.UI",
                        DisplayName = "Category",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Module"
                    },
                    new PageArgs
                    {
                        PageCode = PageArgsCodes.CATEGORIES,
                        ComponentName = "Atlas.Blazor.UI.Components.Administration.CategoriesView, Atlas.Blazor.UI",
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
                        ComponentName = "Atlas.Blazor.UI.Components.Administration.CategoryView, Atlas.Blazor.UI",
                        DisplayName = "Category",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Category"
                    },
                    new PageArgs
                    {
                        PageCode = PageArgsCodes.MENU_ITEMS,
                        ComponentName = "Atlas.Blazor.UI.Components.Administration.MenuItemsView, Atlas.Blazor.UI",
                        DisplayName = "Menu Items",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "MenuItem",
                        ModelParameters = new()
                        {
                            {
                                "Fields", new List<string> { "MenuItemId", "Name", "Permission" }
                            },
                            {
                                "IdentifierField", "MenuItemId"
                            }
                        }
                    },
                    new PageArgs
                    {
                        PageCode = PageArgsCodes.MENU_ITEM,
                        ComponentName = "Atlas.Blazor.UI.Components.Administration.MenuItemView, Atlas.Blazor.UI",
                        DisplayName = "Menu Item",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "MenuItem"
                    }
            };

            return pageArgs;
        }
    }
}
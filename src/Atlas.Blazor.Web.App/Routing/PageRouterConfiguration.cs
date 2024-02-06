using Atlas.Blazor.Web.App.Models;
using Atlas.Core.Constants;
using System.Collections.Generic;

namespace Atlas.Blazor.Web.App.Routing
{
    public static class PageRouterConfiguration
    {
        public static IEnumerable<PageArgs> GetPageArgs()
        {
            List<PageArgs> pageArgs = new()
            {
                    new PageArgs
                    {
                        PageCode = PageCodes.MODULES,
                        ComponentName = "Atlas.Blazor.Web.App.Components.Generic.Administration.ModulesView, Atlas.Blazor.Web.App",
                        DisplayName = "Modules",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Module",
                        ModelParameters = new()
                        {
                            {
                                //"Fields", "ModuleId;Name;Permission"
                                "Fields", "Name;Permission"
                            },
                            {
                                "IdentifierField", "ModuleId"
                            }
                        }
                    },
                    new PageArgs
                    {
                        PageCode = PageCodes.MODULE,
                        ComponentName = "Atlas.Blazor.Web.App.Components.Generic.Administration.ModuleView, Atlas.Blazor.Web.App",
                        DisplayName = "Category",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Module"
                    },
                    new PageArgs
                    {
                        PageCode = PageCodes.CATEGORIES,
                        ComponentName = "Atlas.Blazor.Web.App.Components.Generic.Administration.CategoriesView, Atlas.Blazor.Web.App",
                        DisplayName = "Categories",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Category",
                        ModelParameters = new()
                        {
                            {
                                "Fields", "CategoryId;Name;Permission"
                            },
                            {
                                "IdentifierField", "CategoryId"
                            }
                        }
                    },
                    new PageArgs
                    {
                        PageCode = PageCodes.CATEGORY,
                        ComponentName = "Atlas.Blazor.Web.App.Components.Generic.Administration.CategoryView, Atlas.Blazor.Web.App",
                        DisplayName = "Category",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Category"
                    },
                    new PageArgs
                    {
                        PageCode = PageCodes.MENU_ITEMS,
                        ComponentName = "Atlas.Blazor.Web.App.Components.Generic.Administration.MenuItemsView, Atlas.Blazor.Web.App",
                        DisplayName = "Menu Items",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "MenuItem",
                        ModelParameters = new()
                        {
                            {
                                "Fields", "MenuItemId;Name;Permission"
                            },
                            {
                                "IdentifierField", "MenuItemId"
                            }
                        }
                    },
                    new PageArgs
                    {
                        PageCode = PageCodes.MENU_ITEM,
                        ComponentName = "Atlas.Blazor.Web.App.Components.Generic.Administration.MenuItemView, Atlas.Blazor.Web.App",
                        DisplayName = "Menu Item",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "MenuItem"
                    },
                    new PageArgs
                    {
                        PageCode = PageCodes.USERS,
                        ComponentName = "Atlas.Blazor.Web.App.Components.Generic.Administration.UsersView, Atlas.Blazor.Web.App",
                        DisplayName = "Users",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "User",
                        ModelParameters = new()
                        {
                            {
                                "Fields", "UserId;UserName;Email"
                            },
                            {
                                "IdentifierField", "UserId"
                            }
                        }
                    },
                    new PageArgs
                    {
                        PageCode = PageCodes.USER,
                        ComponentName = "Atlas.Blazor.Web.App.Components.Generic.Administration.UserView, Atlas.Blazor.Web.App",
                        DisplayName = "User",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "User"
                    },
                    new PageArgs
                    {
                        PageCode = PageCodes.ROLES,
                        ComponentName = "Atlas.Blazor.Web.App.Components.Generic.Administration.RolesView, Atlas.Blazor.Web.App",
                        DisplayName = "Roles",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Role",
                        ModelParameters = new()
                        {
                            {
                                "Fields", "RoleId;Name"
                            },
                            {
                                "IdentifierField", "RoleId"
                            }
                        }
                    },
                    new PageArgs
                    {
                        PageCode = PageCodes.ROLE,
                        ComponentName = "Atlas.Blazor.Web.App.Components.Generic.Administration.RoleView, Atlas.Blazor.Web.App",
                        DisplayName = "Role",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Role"
                    },
                    new PageArgs
                    {
                        PageCode = PageCodes.PERMISSIONS,
                        ComponentName = "Atlas.Blazor.Web.App.Components.Generic.Administration.PermissionsView, Atlas.Blazor.Web.App",
                        DisplayName = "Permissions",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Permission",
                        ModelParameters = new()
                        {
                            {
                                "Fields", "PermissionId;Name"
                            },
                            {
                                "IdentifierField", "PermissionId"
                            }
                        }
                    },
                    new PageArgs
                    {
                        PageCode = PageCodes.PERMISSION,
                        ComponentName = "Atlas.Blazor.Web.App.Components.Generic.Administration.PermissionView, Atlas.Blazor.Web.App",
                        DisplayName = "Permission",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Permission"
                    }
            };

            // Additional Modules
            pageArgs.Add(new PageArgs
            {
                PageCode = PageCodes.WEATHER_DISPLAY,
                ComponentName = "Weather.Client.Components.WeatherDisplay, Weather.Client",
                DisplayName = "Weather Forecast",
                RoutingPage = "PageRouter",
                RoutingPageCode = PageCodes.WEATHER_DISPLAY
            });

            return pageArgs;
        }
    }
}
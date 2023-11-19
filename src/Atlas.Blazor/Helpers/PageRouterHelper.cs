using Atlas.Blazor.Models;
using Atlas.Core.Constants;

namespace Atlas.Blazor.Helpers
{
    public static class PageRouterHelper
    {
        public static IEnumerable<PageArgs> GetPageArgs()
        {
            List<PageArgs> pageArgs = new()
            {
                    new PageArgs
                    {
                        PageCode = PageCodes.MODULES,
                        ComponentName = "Atlas.Blazor.Components.Administration.ModulesView, Atlas.Blazor",
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
                        PageCode = PageCodes.MODULE,
                        ComponentName = "Atlas.Blazor.Components.Administration.ModuleView, Atlas.Blazor",
                        DisplayName = "Category",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Module"
                    },
                    new PageArgs
                    {
                        PageCode = PageCodes.CATEGORIES,
                        ComponentName = "Atlas.Blazor.Components.Administration.CategoriesView, Atlas.Blazor",
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
                        PageCode = PageCodes.CATEGORY,
                        ComponentName = "Atlas.Blazor.Components.Administration.CategoryView, Atlas.Blazor",
                        DisplayName = "Category",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Category"
                    },
                    new PageArgs
                    {
                        PageCode = PageCodes.MENU_ITEMS,
                        ComponentName = "Atlas.Blazor.Components.Administration.MenuItemsView, Atlas.Blazor",
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
                        PageCode = PageCodes.MENU_ITEM,
                        ComponentName = "Atlas.Blazor.Components.Administration.MenuItemView, Atlas.Blazor",
                        DisplayName = "Menu Item",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "MenuItem"
                    },
                    new PageArgs
                    {
                        PageCode = PageCodes.USERS,
                        ComponentName = "Atlas.Blazor.Components.Administration.UsersView, Atlas.Blazor",
                        DisplayName = "Users",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "User",
                        ModelParameters = new()
                        {
                            {
                                "Fields", new List<string> { "UserId", "UserName", "Email" }
                            },
                            {
                                "IdentifierField", "UserId"
                            }
                        }
                    },
                    new PageArgs
                    {
                        PageCode = PageCodes.USER,
                        ComponentName = "Atlas.Blazor.Components.Administration.UserView, Atlas.Blazor",
                        DisplayName = "User",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "User"
                    },
                    new PageArgs
                    {
                        PageCode = PageCodes.ROLES,
                        ComponentName = "Atlas.Blazor.Components.Administration.RolesView, Atlas.Blazor",
                        DisplayName = "Roles",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Role",
                        ModelParameters = new()
                        {
                            {
                                "Fields", new List<string> { "RoleId", "Name" }
                            },
                            {
                                "IdentifierField", "RoleId"
                            }
                        }
                    },
                    new PageArgs
                    {
                        PageCode = PageCodes.ROLE,
                        ComponentName = "Atlas.Blazor.Components.Administration.RoleView, Atlas.Blazor",
                        DisplayName = "Role",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Role"
                    },
                    new PageArgs
                    {
                        PageCode = PageCodes.PERMISSIONS,
                        ComponentName = "Atlas.Blazor.Components.Administration.PermissionsView, Atlas.Blazor",
                        DisplayName = "Permissions",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Permission",
                        ModelParameters = new()
                        {
                            {
                                "Fields", new List<string> { "PermissionId", "Name" }
                            },
                            {
                                "IdentifierField", "PermissionId"
                            }
                        }
                    },
                    new PageArgs
                    {
                        PageCode = PageCodes.PERMISSION,
                        ComponentName = "Atlas.Blazor.Components.Administration.PermissionView, Atlas.Blazor",
                        DisplayName = "Permission",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Permission"
                    }
            };

            return pageArgs;
        }
    }
}
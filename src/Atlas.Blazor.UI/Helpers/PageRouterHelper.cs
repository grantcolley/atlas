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
                    },
                    new PageArgs
                    {
                        PageCode = PageArgsCodes.USERS,
                        ComponentName = "Atlas.Blazor.UI.Components.Administration.UsersView, Atlas.Blazor.UI",
                        DisplayName = "Users",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "User",
                        ModelParameters = new()
                        {
                            {
                                "Fields", new List<string> { "UserId", "Name", "Email" }
                            },
                            {
                                "IdentifierField", "UserId"
                            }
                        }
                    },
                    new PageArgs
                    {
                        PageCode = PageArgsCodes.USER,
                        ComponentName = "Atlas.Blazor.UI.Components.Administration.UserView, Atlas.Blazor.UI",
                        DisplayName = "User",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "User"
                    },
                    new PageArgs
                    {
                        PageCode = PageArgsCodes.ROLES,
                        ComponentName = "Atlas.Blazor.UI.Components.Administration.RolesView, Atlas.Blazor.UI",
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
                        PageCode = PageArgsCodes.ROLE,
                        ComponentName = "Atlas.Blazor.UI.Components.Administration.RoleView, Atlas.Blazor.UI",
                        DisplayName = "Role",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Role"
                    },
                    new PageArgs
                    {
                        PageCode = PageArgsCodes.PERMISSIONS,
                        ComponentName = "Atlas.Blazor.UI.Components.Administration.PermissionsView, Atlas.Blazor.UI",
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
                        PageCode = PageArgsCodes.PERMISSION,
                        ComponentName = "Atlas.Blazor.UI.Components.Administration.PermissionView, Atlas.Blazor.UI",
                        DisplayName = "Permission",
                        RoutingPage = "PageRouter",
                        RoutingPageCode = "Permission"
                    }
            };

            return pageArgs;
        }
    }
}
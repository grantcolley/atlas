using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Data.Access.Context;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Seed.Data
{
    public class SeedData
    {
        private static ApplicationDbContext? dbContext;

        private static readonly Dictionary<string, Permission> permissions = new();
        private static readonly Dictionary<string, Role> roles = new();
        private static readonly Dictionary<string, User> users = new();

        public static void Initialise(ApplicationDbContext applicationDbContext)
        {
            dbContext = applicationDbContext ?? throw new NullReferenceException(nameof(applicationDbContext));

            TruncateTables();

            CreatePermissions();
            CreateRoles();
            CreateUsers();
            AssignUsersRoles();

            Navigation();

            AddWeatherModule();
        }

        private static void TruncateTables()
        {
            if (dbContext == null) throw new NullReferenceException(nameof(dbContext));

            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE Audits");
            //((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE Logs");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE RoleUser");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE PermissionUser");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE PermissionRole");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Users");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Users, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Roles");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Roles, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Permissions");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Permissions, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE MenuItems");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Categories");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Categories, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Modules");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Modules, RESEED, 1)");
        }

        private static void CreatePermissions()
        {
            if (dbContext == null) throw new NullReferenceException(nameof(dbContext));

            permissions.Add(Auth.USER, new Permission { Name = Auth.USER, Description = "Atlas User Permission" });
            permissions.Add(Auth.ADMIN, new Permission { Name = Auth.ADMIN, Description = "Atlas Administrator Permission" });
            permissions.Add(Auth.DEVELOPER, new Permission { Name = Auth.DEVELOPER, Description = "Atlas Developer Permission" });
            permissions.Add(Auth.WEATHER_USER, new Permission { Name = Auth.WEATHER_USER, Description = "Weather User Permission" });

            foreach (var permission in permissions.Values)
            {
                dbContext.Permissions.Add(permission);
            }

            dbContext.SaveChanges();
        }

        private static void CreateRoles()
        {
            if (dbContext == null) throw new NullReferenceException(nameof(dbContext));

            roles.Add(Auth.USER, new Role { Name = Auth.USER, Description = "Atlas User Role" });
            roles.Add(Auth.ADMIN, new Role { Name = Auth.ADMIN, Description = "Atlas Administrator Role" });
            roles.Add(Auth.DEVELOPER, new Role { Name = Auth.DEVELOPER, Description = "Atlas Developer Role" });
            roles.Add(Auth.WEATHER_USER, new Role { Name = Auth.WEATHER_USER, Description = "Weather User Role" });

            foreach (var role in roles.Values)
            {
                dbContext.Roles.Add(role);
            }

            roles[Auth.USER].Permissions.Add(permissions[Auth.USER]);

            roles[Auth.WEATHER_USER].Permissions.Add(permissions[Auth.USER]);
            roles[Auth.WEATHER_USER].Permissions.Add(permissions[Auth.WEATHER_USER]);

            roles[Auth.ADMIN].Permissions.Add(permissions[Auth.USER]);
            roles[Auth.ADMIN].Permissions.Add(permissions[Auth.ADMIN]);
            roles[Auth.ADMIN].Permissions.Add(permissions[Auth.WEATHER_USER]);

            roles[Auth.DEVELOPER].Permissions.Add(permissions[Auth.USER]);
            roles[Auth.DEVELOPER].Permissions.Add(permissions[Auth.ADMIN]);
            roles[Auth.DEVELOPER].Permissions.Add(permissions[Auth.DEVELOPER]);
            roles[Auth.DEVELOPER].Permissions.Add(permissions[Auth.WEATHER_USER]);

            dbContext.SaveChanges();
        }

        private static void CreateUsers()
        {
            if (dbContext == null) throw new NullReferenceException(nameof(dbContext));

            users.Add("alice", new User { UserName = "alice", Email = "alice@email.com", Theme = "LightMode" });
            users.Add("bob", new User { UserName = "bob", Email = "bob@email.com", Theme = "LightMode" });
            users.Add("grant", new User { UserName = "grant", Email = "grant@email.com", Theme = "DarkMode" });

            foreach (var user in users.Values)
            {
                dbContext.Users.Add(user);
            }

            dbContext.SaveChanges();
        }

        private static void AssignUsersRoles()
        {
            if (dbContext == null) throw new NullReferenceException(nameof(dbContext));

            users["alice"].Roles.AddRange(new[] { roles[Auth.ADMIN], roles[Auth.WEATHER_USER] });
            users["bob"].Roles.AddRange(new[] { roles[Auth.USER], roles[Auth.WEATHER_USER] });
            users["grant"].Roles.Add(roles[Auth.DEVELOPER]);

            dbContext.SaveChanges();
        }

        private static void Navigation()
        {
            if (dbContext == null) throw new NullReferenceException(nameof(dbContext));

            var administration = new Module { Name = "Administration", Icon = "Engineering", Order = 2, Permission = Auth.ADMIN };

            dbContext.Modules.Add(administration);

            dbContext.SaveChanges();

            var authorisationCategory = new Category { Name = "Authorisation", Icon = "AdminPanelSettings", Order = 1, Permission = Auth.ADMIN, Module = administration };
            var navigationCategory = new Category { Name = "Navigation", Icon = "Explore", Order = 2, Permission = Auth.DEVELOPER, Module = administration };

            administration.Categories.Add(authorisationCategory);
            administration.Categories.Add(navigationCategory);

            dbContext.Categories.Add(authorisationCategory);
            dbContext.Categories.Add(navigationCategory);

            dbContext.SaveChanges();

            var usersMenuItem = new MenuItem { Name = "Users", Icon = "SupervisedUserCircle", NavigatePage = "PageRouter", Order = 1, Permission = Auth.ADMIN, Category = authorisationCategory, PageCode = PageCodes.USERS };
            var rolesMenuItem = new MenuItem { Name = "Roles", Icon = "Lock", NavigatePage = "PageRouter", Order = 2, Permission = Auth.ADMIN, Category = authorisationCategory, PageCode = PageCodes.ROLES };
            var permissionsMenuItem = new MenuItem { Name = "Permissions", Icon = "Key", NavigatePage = "PageRouter", Order = 3, Permission = Auth.ADMIN, Category = authorisationCategory, PageCode = PageCodes.PERMISSIONS };

            var modulesMenuItem = new MenuItem { Name = "Modules", Icon = "AutoAwesomeMosaic", NavigatePage = "PageRouter", Order = 1, Permission = Auth.DEVELOPER, Category = navigationCategory, PageCode = PageCodes.MODULES };
            var categoriesMenuItem = new MenuItem { Name = "Categories", Icon = "AutoAwesomeMotion", NavigatePage = "PageRouter", Order = 2, Permission = Auth.DEVELOPER, Category = navigationCategory, PageCode = PageCodes.CATEGORIES };
            var menuItemsMenuItem = new MenuItem { Name = "MenuItems", Icon = "Article", NavigatePage = "PageRouter", Order = 3, Permission = Auth.DEVELOPER, Category = navigationCategory, PageCode = PageCodes.MENU_ITEMS };
            
            authorisationCategory.MenuItems.Add(usersMenuItem);
            authorisationCategory.MenuItems.Add(rolesMenuItem);
            authorisationCategory.MenuItems.Add(permissionsMenuItem);

            navigationCategory.MenuItems.Add(modulesMenuItem);
            navigationCategory.MenuItems.Add(categoriesMenuItem);
            navigationCategory.MenuItems.Add(menuItemsMenuItem);

            dbContext.MenuItems.Add(usersMenuItem);
            dbContext.MenuItems.Add(rolesMenuItem);
            dbContext.MenuItems.Add(permissionsMenuItem);
            dbContext.MenuItems.Add(modulesMenuItem);
            dbContext.MenuItems.Add(categoriesMenuItem);
            dbContext.MenuItems.Add(menuItemsMenuItem);

            dbContext.SaveChanges();
        }

        private static void AddWeatherModule()
        {
            if (dbContext == null) throw new NullReferenceException(nameof(dbContext));

            var weather = new Module { Name = "Weather", Icon = "Thunderstorm", Order = 1, Permission = Auth.WEATHER_USER };

            dbContext.Modules.Add(weather);

            dbContext.SaveChanges();

            var forecastCategory = new Category { Name = "Forecast", Icon = "WbCloudy", Order = 1, Permission = Auth.WEATHER_USER, Module = weather };

            weather.Categories.Add(forecastCategory);

            dbContext.Categories.Add(forecastCategory);

            dbContext.SaveChanges();

            var weatherForecastMenuItem = new MenuItem { Name = "Weather Display", Icon = "DeviceThermostat", NavigatePage = "PageRouter", Order = 1, Permission = Auth.WEATHER_USER, Category = forecastCategory, PageCode = PageCodes.WEATHER_DISPLAY };

            forecastCategory.MenuItems.Add(weatherForecastMenuItem);

            dbContext.MenuItems.Add(weatherForecastMenuItem);

            dbContext.SaveChanges();
        }
    }
}
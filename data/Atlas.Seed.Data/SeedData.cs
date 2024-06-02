using Atlas.Blazor.Web.Constants;
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

            //AddWeatherModule();
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

            permissions.Add(Auth.USER, new Permission { Code = Auth.USER, Name = Auth.USER, Description = "Atlas User Permission" });
            permissions.Add(Auth.ADMIN, new Permission { Code = Auth.ADMIN, Name = Auth.ADMIN, Description = "Atlas Administrator Permission" });
            permissions.Add(Auth.DEVELOPER, new Permission { Code = Auth.DEVELOPER, Name = Auth.DEVELOPER, Description = "Atlas Developer Permission" });
            permissions.Add(Auth.WEATHER_USER, new Permission { Code = Auth.WEATHER_USER, Name = Auth.WEATHER_USER, Description = "Weather User Permission" });

            foreach (var permission in permissions.Values)
            {
                dbContext.Permissions.Add(permission);
            }

            dbContext.SaveChanges();
        }

        private static void CreateRoles()
        {
            if (dbContext == null) throw new NullReferenceException(nameof(dbContext));

            roles.Add(Auth.USER, new Role { Name = $"{Auth.USER} Role", Description = $"{Auth.USER} Role" });
            roles.Add(Auth.ADMIN, new Role { Name = $"{Auth.ADMIN} Role", Description = $"{Auth.ADMIN} Role" });
            roles.Add(Auth.DEVELOPER, new Role { Name = $"{Auth.DEVELOPER} Role", Description = $"{Auth.DEVELOPER} Role" });
            roles.Add(Auth.WEATHER_USER, new Role { Name = $"{Auth.WEATHER_USER} Role", Description = $"{Auth.WEATHER_USER} Role" });

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

            users.Add("alice", new User { Name = "alice", Email = "alice@email.com", Theme = "LightMode" });
            users.Add("bob", new User { Name = "bob", Email = "bob@email.com", Theme = "LightMode" });
            users.Add("grant", new User { Name = "grant", Email = "grant@email.com", Theme = "DarkMode" });

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

            var administration = new Module { Name = "Administration", Icon = "TableSettings", Order = 2, Permission = Auth.ADMIN };

            dbContext.Modules.Add(administration);

            dbContext.SaveChanges();

            var authorisationCategory = new Category { Name = "Authorisation", Icon = "ShieldLock", Order = 1, Permission = Auth.ADMIN, Module = administration };

            administration.Categories.Add(authorisationCategory);

            dbContext.Categories.Add(authorisationCategory);

            dbContext.SaveChanges();

            var usersMenuItem = new MenuItem { Name = "Users", Icon = "PeopleLock", NavigatePage = AtlasWebConstants.PAGE_USERS, Order = 1, Permission = Auth.ADMIN, Category = authorisationCategory };
            var rolesMenuItem = new MenuItem { Name = "Roles", Icon = "LockMultiple", NavigatePage = AtlasWebConstants.PAGE_ROLES, Order = 2, Permission = Auth.ADMIN, Category = authorisationCategory };
            var permissionsMenuItem = new MenuItem { Name = "Permissions", Icon = "KeyMultiple", NavigatePage = AtlasWebConstants.PAGE_PERMISSIONS, Order = 3, Permission = Auth.ADMIN, Category = authorisationCategory };
            
            authorisationCategory.MenuItems.Add(usersMenuItem);
            authorisationCategory.MenuItems.Add(rolesMenuItem);
            authorisationCategory.MenuItems.Add(permissionsMenuItem);

            dbContext.MenuItems.Add(usersMenuItem);
            dbContext.MenuItems.Add(rolesMenuItem);
            dbContext.MenuItems.Add(permissionsMenuItem);

            dbContext.SaveChanges();
        }

        //private static void AddWeatherModule()
        //{
        //    if (dbContext == null) throw new NullReferenceException(nameof(dbContext));

        //    var weather = new Module { Name = "Weather", Icon = "Thunderstorm", Order = 1, Permission = Auth.WEATHER_USER };

        //    dbContext.Modules.Add(weather);

        //    dbContext.SaveChanges();

        //    var forecastCategory = new Category { Name = "Forecast", Icon = "WbCloudy", Order = 1, Permission = Auth.WEATHER_USER, Module = weather };

        //    weather.Categories.Add(forecastCategory);

        //    dbContext.Categories.Add(forecastCategory);

        //    dbContext.SaveChanges();

        //    var weatherForecastMenuItem = new MenuItem { Name = "Weather Display", Icon = "DeviceThermostat", NavigatePage = "PageRouter", Order = 1, Permission = Auth.WEATHER_USER, Category = forecastCategory, PageCode = PageCodes.WEATHER_DISPLAY };

        //    forecastCategory.MenuItems.Add(weatherForecastMenuItem);

        //    dbContext.MenuItems.Add(weatherForecastMenuItem);

        //    dbContext.SaveChanges();
        //}
    }
}
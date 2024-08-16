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

        private static readonly Dictionary<string, Permission> permissions = [];
        private static readonly Dictionary<string, Role> roles = [];
        private static readonly Dictionary<string, User> users = [];

        public static void Generate(ApplicationDbContext applicationDbContext)
        {
            dbContext = applicationDbContext ?? throw new NullReferenceException(nameof(applicationDbContext));

            TruncateTables();
            CreatePermissions();
            CreateRoles();
            CreateUsers();
            AssignUsersRoles();
            AddApplications();
        }

        private static void TruncateTables()
        {
            if (dbContext == null) throw new NullReferenceException(nameof(dbContext));

            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE Audits");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE Logs");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE RoleUser");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE PermissionRole");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Users");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Users, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Roles");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Roles, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Permissions");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Permissions, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE Pages");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Categories");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Categories, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Modules");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Modules, RESEED, 1)");
        }

        private static void CreatePermissions()
        {
            if (dbContext == null) throw new NullReferenceException(nameof(dbContext));

            permissions.Add(Auth.ADMIN_READ, new Permission { Code = Auth.ADMIN_READ, Name = Auth.ADMIN_READ, Description = "Atlas Administrator Read Permission" });
            permissions.Add(Auth.ADMIN_WRITE, new Permission { Code = Auth.ADMIN_WRITE, Name = Auth.ADMIN_WRITE, Description = "Atlas Administrator Write Permission" });
            permissions.Add(Auth.SUPPORT, new Permission { Code = Auth.SUPPORT, Name = Auth.SUPPORT, Description = "Atlas Support Permission" });
            permissions.Add(Auth.DEVELOPER, new Permission { Code = Auth.DEVELOPER, Name = Auth.DEVELOPER, Description = "Atlas Developer Permission" });

            foreach (Permission permission in permissions.Values)
            {
                dbContext.Permissions.Add(permission);
            }

            dbContext.SaveChanges();
        }

        private static void CreateRoles()
        {
            if (dbContext == null) throw new NullReferenceException(nameof(dbContext));

            roles.Add(Auth.ADMIN_READ, new Role { Name = $"{Auth.ADMIN_READ} Role", Description = $"{Auth.ADMIN_READ} Role" });
            roles.Add(Auth.ADMIN_WRITE, new Role { Name = $"{Auth.ADMIN_WRITE} Role", Description = $"{Auth.ADMIN_WRITE} Role" });
            roles.Add(Auth.SUPPORT, new Role { Name = $"{Auth.SUPPORT} Role", Description = $"{Auth.SUPPORT} Role" });
            roles.Add(Auth.DEVELOPER, new Role { Name = $"{Auth.DEVELOPER} Role", Description = $"{Auth.DEVELOPER} Role" });

            foreach (Role role in roles.Values)
            {
                dbContext.Roles.Add(role);
            }

            roles[Auth.ADMIN_READ].Permissions.Add(permissions[Auth.ADMIN_READ]);

            roles[Auth.ADMIN_WRITE].Permissions.Add(permissions[Auth.ADMIN_READ]);
            roles[Auth.ADMIN_WRITE].Permissions.Add(permissions[Auth.ADMIN_WRITE]);

            roles[Auth.SUPPORT].Permissions.Add(permissions[Auth.SUPPORT]);
            roles[Auth.SUPPORT].Permissions.Add(permissions[Auth.ADMIN_READ]);
            roles[Auth.SUPPORT].Permissions.Add(permissions[Auth.ADMIN_WRITE]);

            roles[Auth.DEVELOPER].Permissions.Add(permissions[Auth.ADMIN_READ]);
            roles[Auth.DEVELOPER].Permissions.Add(permissions[Auth.ADMIN_WRITE]);
            roles[Auth.DEVELOPER].Permissions.Add(permissions[Auth.SUPPORT]);
            roles[Auth.DEVELOPER].Permissions.Add(permissions[Auth.DEVELOPER]);

            dbContext.SaveChanges();
        }

        private static void CreateUsers()
        {
            if (dbContext == null) throw new NullReferenceException(nameof(dbContext));

            users.Add("alice", new User { Name = "alice", Email = "alice@email.com" });
            users.Add("jane", new User { Name = "jane", Email = "jane@email.com" });
            users.Add("bob", new User { Name = "bob", Email = "bob@email.com" });
            users.Add("grant", new User { Name = "grant", Email = "grant@email.com" });

            foreach (User user in users.Values)
            {
                dbContext.Users.Add(user);
            }

            dbContext.SaveChanges();
        }

        private static void AssignUsersRoles()
        {
            if (dbContext == null) throw new NullReferenceException(nameof(dbContext));

            users["alice"].Roles.AddRange([roles[Auth.ADMIN_WRITE]]);
            users["jane"].Roles.AddRange([roles[Auth.ADMIN_READ]]);
            users["bob"].Roles.AddRange([roles[Auth.SUPPORT]]);
            users["grant"].Roles.Add(roles[Auth.DEVELOPER]);

            dbContext.SaveChanges();
        }

        private static void AddApplications()
        {
            AddAdministration();
            AddSupport();
        }

        private static void AddAdministration()
        {
            if (dbContext == null) throw new NullReferenceException(nameof(dbContext));

            Module administrationModule = new() { Name = "Administration", Icon = "TableSettings", Order = 1, Permission = Auth.ADMIN_READ };

            dbContext.Modules.Add(administrationModule);

            dbContext.SaveChanges();

            AddAuthorisationCategory(administrationModule);
            AddDevelopmentCategory(administrationModule);
        }

        private static void AddAuthorisationCategory(Module administrationModule)
        {
            ArgumentNullException.ThrowIfNull(administrationModule);
            if (dbContext == null) throw new NullReferenceException(nameof(dbContext));

            Category authorisationCategory = new() { Name = "Authorisation", Icon = "ShieldLock", Order = 1, Permission = Auth.ADMIN_READ, Module = administrationModule };

            administrationModule.Categories.Add(authorisationCategory);

            dbContext.Categories.Add(authorisationCategory);

            dbContext.SaveChanges();

            Page usersPage = new() { Name = "Users", Icon = "PeopleLock", Route = AtlasWebConstants.PAGE_USERS, Order = 1, Permission = Auth.ADMIN_READ, Category = authorisationCategory };
            Page rolesPage = new() { Name = "Roles", Icon = "LockMultiple", Route = AtlasWebConstants.PAGE_ROLES, Order = 2, Permission = Auth.ADMIN_READ, Category = authorisationCategory };
            Page permissionsPage = new() { Name = "Permissions", Icon = "KeyMultiple", Route = AtlasWebConstants.PAGE_PERMISSIONS, Order = 3, Permission = Auth.ADMIN_READ, Category = authorisationCategory };

            authorisationCategory.Pages.Add(usersPage);
            authorisationCategory.Pages.Add(rolesPage);
            authorisationCategory.Pages.Add(permissionsPage);

            dbContext.Pages.Add(usersPage);
            dbContext.Pages.Add(rolesPage);
            dbContext.Pages.Add(permissionsPage);

            dbContext.SaveChanges();
        }

        private static void AddDevelopmentCategory(Module administrationModule)
        {
            ArgumentNullException.ThrowIfNull(administrationModule);
            if (dbContext == null) throw new NullReferenceException(nameof(dbContext));

            Category configurationCategory = new() { Name = "Applications", Icon = "Apps", Order = 2, Permission = Auth.DEVELOPER, Module = administrationModule };

            administrationModule.Categories.Add(configurationCategory);

            dbContext.Categories.Add(configurationCategory);

            dbContext.SaveChanges();

            Page modulePage = new() { Name = "Modules", Icon = "PanelLeftText", Route = AtlasWebConstants.PAGE_MODULES, Order = 1, Permission = Auth.DEVELOPER, Category = configurationCategory };
            Page categoriesPage = new() { Name = "Categories", Icon = "AppsListDetail", Route = AtlasWebConstants.PAGE_CATEGORIES, Order = 2, Permission = Auth.DEVELOPER, Category = configurationCategory };
            Page pagesPage = new() { Name = "Pages", Icon = "DocumentOnePage", Route = AtlasWebConstants.PAGE_PAGES, Order = 3, Permission = Auth.DEVELOPER, Category = configurationCategory };

            configurationCategory.Pages.Add(modulePage);
            configurationCategory.Pages.Add(categoriesPage);
            configurationCategory.Pages.Add(pagesPage);

            dbContext.Pages.Add(modulePage);
            dbContext.Pages.Add(categoriesPage);
            dbContext.Pages.Add(pagesPage);

            dbContext.SaveChanges();
        }

        private static void AddSupport()
        {
            if (dbContext == null) throw new NullReferenceException(nameof(dbContext));

            Module supportModule = new() { Name = "Support", Icon = "PersonSupport", Order = 2, Permission = Auth.SUPPORT };

            dbContext.Modules.Add(supportModule);

            dbContext.SaveChanges();

            Category eventCategory = new() { Name = "Events", Icon = "ClockToolbox", Order = 1, Permission = Auth.SUPPORT, Module = supportModule };

            supportModule.Categories.Add(eventCategory);

            dbContext.Categories.Add(eventCategory);

            dbContext.SaveChanges();

            Page logsPage = new() { Name = "Logs", Icon = "DocumentTextClock", Route = AtlasWebConstants.PAGE_LOGS, Order = 1, Permission = Auth.SUPPORT, Category = eventCategory };

            eventCategory.Pages.Add(logsPage);

            dbContext.Pages.Add(logsPage);

            dbContext.SaveChanges();
        }

        //private static void AddWeatherModule()
        //{
        //    if (dbContext == null) throw new NullReferenceException(nameof(dbContext));

        //    Module weather = new() { Name = "Weather", Icon = "Thunderstorm", Order = 1, Permission = Auth.WEATHER_USER };

        //    dbContext.Modules.Add(weather);

        //    dbContext.SaveChanges();

        //    Category forecastCategory = new() { Name = "Forecast", Icon = "WbCloudy", Order = 1, Permission = Auth.WEATHER_USER, Module = weather };

        //    weather.Categories.Add(forecastCategory);

        //    dbContext.Categories.Add(forecastCategory);

        //    dbContext.SaveChanges();

        //    Page weatherForecastPage = new() { Name = "Weather Display", Icon = "DeviceThermostat", NavigatePage = "PageRouter", Order = 1, Permission = Auth.WEATHER_USER, Category = forecastCategory, PageCode = PageCodes.WEATHER_DISPLAY };

        //    forecastCategory.Pages.Add(weatherForecastPage);

        //    dbContext.Pages.Add(weatherForecastPage);

        //    dbContext.SaveChanges();
        //}
    }
}
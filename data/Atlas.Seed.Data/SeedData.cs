using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Data.Access.Context;
using Microsoft.EntityFrameworkCore;

namespace Atlas.Seed.Data
{
    public class SeedData
    {
        private static ApplicationDbContext dbContext;

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
        }

        private static void TruncateTables()
        {
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE Audits");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE Logs");
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
            permissions.Add(Auth.USER, new Permission { Name = Auth.USER, Description = "Atlas User Permission" });
            permissions.Add(Auth.ADMIN, new Permission { Name = Auth.ADMIN, Description = "Atlas Administrator Permission" });
            permissions.Add(Auth.DEVELOPER, new Permission { Name = Auth.DEVELOPER, Description = "Atlas Developer Permission" });

            foreach (var permission in permissions.Values)
            {
                dbContext.Permissions.Add(permission);
            }

            dbContext.SaveChanges();
        }

        private static void CreateRoles()
        {
            roles.Add(Auth.USER, new Role { Name = Auth.USER, Description = "Atlas User Role" });
            roles.Add(Auth.ADMIN, new Role { Name = Auth.ADMIN, Description = "Atlas Administrator Role" });
            roles.Add(Auth.DEVELOPER, new Role { Name = Auth.DEVELOPER, Description = "Atlas Developer Role" });

            foreach (var role in roles.Values)
            {
                dbContext.Roles.Add(role);
            }

            roles[Auth.USER].Permissions.Add(permissions[Auth.USER]);

            roles[Auth.ADMIN].Permissions.Add(permissions[Auth.USER]);
            roles[Auth.ADMIN].Permissions.Add(permissions[Auth.ADMIN]);

            roles[Auth.DEVELOPER].Permissions.Add(permissions[Auth.USER]);
            roles[Auth.DEVELOPER].Permissions.Add(permissions[Auth.ADMIN]);
            roles[Auth.DEVELOPER].Permissions.Add(permissions[Auth.DEVELOPER]);

            dbContext.SaveChanges();
        }

        private static void CreateUsers()
        {
            users.Add("alice", new User { UserName = "alice", Email = "alice@email.com" });
            users.Add("bob", new User { UserName = "bob", Email = "bob@email.com" });
            users.Add("grant", new User { UserName = "grant", Email = "grant@email.com" });

            foreach (var user in users.Values)
            {
                dbContext.Users.Add(user);
            }

            dbContext.SaveChanges();
        }

        private static void AssignUsersRoles()
        {
            users["alice"].Roles.Add(roles[Auth.ADMIN]);
            users["bob"].Roles.Add(roles[Auth.USER]);
            users["grant"].Roles.Add(roles[Auth.DEVELOPER]);

            dbContext.SaveChanges();
        }

        private static void Navigation()
        {
            var administration = new Module { Name = "Administration", Icon = "Engineering", Order = 2, Permission = Auth.ADMIN };

            dbContext.Modules.Add(administration);

            var authorisationCatgory = new Category { Name = "Authorisation", Icon = "AdminPanelSettings", Order = 1, Permission = Auth.ADMIN };
            var navigationCatgory = new Category { Name = "Navigation", Icon = "Explore", Order = 2, Permission = Auth.ADMIN };

            dbContext.Categories.Add(authorisationCatgory);

            var usersMenuItem = new MenuItem { Name = "Users", Icon = "SupervisedUserCircle", NavigatePage = "Page", Order = 1, Permission = Auth.ADMIN };
            var rolesMenuItem = new MenuItem { Name = "Roles", Icon = "Lock", NavigatePage = "Page", Order = 2, Permission = Auth.ADMIN };
            var permissionsMenuItem = new MenuItem { Name = "Permissions", Icon = "Key", NavigatePage = "Page", Order = 3, Permission = Auth.ADMIN };

            var modulesMenuItem = new MenuItem { Name = "Modules", Icon = "AutoAwesomeMosaic", NavigatePage = "Page", Order = 1, Permission = Auth.ADMIN };
            var categoriesMenuItem = new MenuItem { Name = "Categories", Icon = "AutoAwesomeMotion", NavigatePage = "Page", Order = 2, Permission = Auth.ADMIN };
            var menuItemsMenuItem = new MenuItem { Name = "MenuItems", Icon = "Article", NavigatePage = "Page", Order = 3, Permission = Auth.ADMIN };

            dbContext.MenuItems.Add(usersMenuItem);
            dbContext.MenuItems.Add(rolesMenuItem);
            dbContext.MenuItems.Add(permissionsMenuItem);
            dbContext.MenuItems.Add(modulesMenuItem);
            dbContext.MenuItems.Add(categoriesMenuItem);
            dbContext.MenuItems.Add(menuItemsMenuItem);

            authorisationCatgory.MenuItems.Add(usersMenuItem);
            authorisationCatgory.MenuItems.Add(rolesMenuItem);
            authorisationCatgory.MenuItems.Add(permissionsMenuItem);
            navigationCatgory.MenuItems.Add(modulesMenuItem);
            navigationCatgory.MenuItems.Add(categoriesMenuItem);
            navigationCatgory.MenuItems.Add(menuItemsMenuItem);

            administration.Categories.Add(authorisationCatgory);
            administration.Categories.Add(navigationCatgory);

            dbContext.SaveChanges();
        }
    }
}
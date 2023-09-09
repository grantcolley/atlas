using Atlas.Core.Models;
using Atlas.Data.Access.Context;
using Atlas.Data.Access.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Atlas.Data.Access.Data
{
    public class NavigationData : DataBase<NavigationData>, INavigationData
    {
        public NavigationData(ApplicationDbContext applicationDbContext, ILogger<NavigationData> logger)
            : base(applicationDbContext, logger)
        {
        }

        public async Task<IEnumerable<Module>?> GetNavigationClaimsAsync(string claim, CancellationToken cancellationToken)
        {
            var user = await applicationDbContext.Users
                .Include(u => u.Permissions)
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.Equals(claim))
                .ConfigureAwait(false);

            var userPermissions = user.Permissions.Select(p => p.Name).ToList();

            var rolePermissions = user.Roles
                .SelectMany(r => r.Permissions)
                .Select(p => p.Name)
                .ToList();

            userPermissions.AddRange(rolePermissions);

            var permissions = userPermissions.Distinct().ToList();

            var modules = await applicationDbContext.Modules
                .AsNoTracking()
                .Include(m => m.Categories.OrderBy(c => c.Order))
                .ThenInclude(c => c.MenuItems.OrderBy(mu => mu.Order))
                .Where(m => permissions.Contains(m.Permission))
                .OrderBy(m => m.Order)
                .ToListAsync()
                .ConfigureAwait(false);

            var permittedModules = modules
                .Where(m => m.IsPermitted(permissions))
                .ToList();

            return permittedModules;
        }
    }
}

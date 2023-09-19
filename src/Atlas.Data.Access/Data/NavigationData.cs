using Atlas.Core.Models;
using Atlas.Data.Access.Base;
using Atlas.Data.Access.Context;
using Atlas.Data.Access.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Atlas.Data.Access.Data
{
    public class NavigationData : AuthorisationData<NavigationData>, INavigationData
    {
        public NavigationData(ApplicationDbContext applicationDbContext, ILogger<NavigationData> logger)
            : base(applicationDbContext, logger)
        {
        }

        public async Task<IEnumerable<Module>?> GetNavigationClaimsAsync(string claim, CancellationToken cancellationToken)
        {
            if(string.IsNullOrWhiteSpace(claim))
            {
                return default;
            }

            User? user = await _applicationDbContext.Users
                .Include(u => u.Permissions)
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email != null && u.Email.Equals(claim), cancellationToken)
                .ConfigureAwait(false);

            if(user == null) 
            {
                return default;
            }

            List<string?> userPermissions = user.Permissions.Select(p => p.Name).ToList();

            List<string?> rolePermissions = user.Roles
                .SelectMany(r => r.Permissions)
                .Select(p => p.Name)
                .ToList();

            userPermissions.AddRange(rolePermissions);

            List<string?> permissions = userPermissions.Distinct().ToList();

            List<Module> modules = await _applicationDbContext.Modules
                .AsNoTracking()
                .Include(m => m.Categories.OrderBy(c => c.Order))
                .ThenInclude(c => c.MenuItems.OrderBy(mu => mu.Order))
                .Where(m => permissions.Contains(m.Permission))
                .OrderBy(m => m.Order)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            List<Module> permittedModules = modules
                .Where(m => m.IsPermitted(permissions))
                .ToList();

            return permittedModules;
        }
    }
}

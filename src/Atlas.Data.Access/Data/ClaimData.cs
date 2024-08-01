using Atlas.Core.Exceptions;
using Atlas.Core.Models;
using Atlas.Data.Access.Base;
using Atlas.Data.Access.Context;
using Atlas.Data.Access.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Atlas.Data.Access.Data
{
    public class ClaimData(ApplicationDbContext applicationDbContext, ILogger<ClaimData> logger) 
        : AuthorisationData<ClaimData>(applicationDbContext, logger), IClaimData
    {
        public async Task<IEnumerable<Module>?> GetApplicationClaimsAsync(CancellationToken cancellationToken)
        {
            string? email = null;

            try
            {
                email = _applicationDbContext.GetUser();

                if (string.IsNullOrWhiteSpace(email))
                {
                    return default;
                }

                User? user = await _applicationDbContext.Users
                    .Include(u => u.Roles)
                    .ThenInclude(r => r.Permissions)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Email != null && u.Email.Equals(email), cancellationToken)
                    .ConfigureAwait(false);

                if (user == null)
                {
                    return default;
                }

                List<string?> rolePermissions = user.Roles
                    .SelectMany(r => r.Permissions)
                    .Select(p => p.Code)
                    .ToList();

                List<string?> permissions = rolePermissions.Distinct().ToList();

                List<Module> modules = await _applicationDbContext.Modules
                    .AsNoTracking()
                    .Include(m => m.Categories.OrderBy(c => c.Order))
                    .ThenInclude(c => c.Pages.OrderBy(mu => mu.Order))
                    .Where(m => permissions.Contains(m.Permission))
                    .OrderBy(m => m.Order)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);

                List<Module> permittedModules = modules
                    .Where(m => m.IsPermitted(permissions))
                    .ToList();

                return permittedModules;
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex, $"User={email ?? "Email is null"}");
            }
        }
    }
}

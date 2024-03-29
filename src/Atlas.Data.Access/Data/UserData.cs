﻿using Atlas.Core.Models;
using Atlas.Data.Access.Base;
using Atlas.Data.Access.Context;
using Atlas.Data.Access.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Atlas.Data.Access.Data
{
    public class UserData : AuthorisationData<UserData>, IUserData
    {
        public UserData(ApplicationDbContext applicationDbContext, ILogger<UserData> logger)
            : base(applicationDbContext, logger)
        {
        }

        public async Task<IEnumerable<Module>?> GetNavigationClaimsAsync(CancellationToken cancellationToken)
        {
            string? email = _applicationDbContext.GetUser();

            if (string.IsNullOrWhiteSpace(email))
            {
                return default;
            }

            User? user = await _applicationDbContext.Users
                .Include(u => u.Permissions)
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email != null && u.Email.Equals(email), cancellationToken)
                .ConfigureAwait(false);

            if (user == null)
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

        public async Task SetThemePreferenceAsync(string theme, CancellationToken cancellationToken)
        {
            if (theme == null) throw new ArgumentNullException(nameof(theme));

            string? email = _applicationDbContext.GetUser();

            if (string.IsNullOrWhiteSpace(email))
            {
                return;
            }

            User? user = await _applicationDbContext.Users
                .FirstOrDefaultAsync(u => u.Email != null && u.Email.Equals(email), cancellationToken)
                .ConfigureAwait(false);

            if (user == null)
            {
                return;
            }

            user.Theme = theme;

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

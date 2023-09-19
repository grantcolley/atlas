using Atlas.Core.Models;
using Atlas.Data.Access.Context;
using Atlas.Data.Access.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Atlas.Data.Access.Base
{
    public abstract class AuthorisationData<T> : DataBase<T>, IAuthorisationData
    {
        public AuthorisationData(ApplicationDbContext applicationDbContext, ILogger<T> logger)
            : base(applicationDbContext, logger)
        {
        }

        public async Task<Authorisation?> GetAuthorisationAsync(string? claim)
        {
            if (string.IsNullOrWhiteSpace(claim))
            {
                return default;
            }

            var user = await _applicationDbContext.Users
                .AsNoTracking()
                .Include(u => u.Permissions)
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(u => u.Email != null && u.Email.Equals(claim))
                .ConfigureAwait(false);

            if (user != null
                && user.Email != null)
            {
                SetUser(user.Email);

                var permissionSet = user.GetUserPermissionSet();

                return new Authorisation { User = claim, Permissions = permissionSet };
            }

            return default;
        }

        private void SetUser(string user)
        {
            _applicationDbContext.SetUser(user);
        }
    }
}

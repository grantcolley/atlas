using Atlas.Core.Models;
using Atlas.Data.Access.Context;
using Atlas.Data.Access.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Atlas.Data.Access.Data
{
    public abstract class DataBase<T> : IData
    {
        protected readonly ApplicationDbContext applicationDbContext;
        protected readonly ILogger<T> logger;
        private bool disposedValue;

        protected string? User;

        protected DataBase(ApplicationDbContext applicationDbContext, ILogger<T> logger)
        {
            this.applicationDbContext = applicationDbContext;
            this.logger = logger;
        }

        public void SetUser(string user)
        {
            User = user;

            applicationDbContext.SetUser(user);
        }

        public async Task<bool> IsAuthorisedAsync(string claim, string permission)
        {
            return await applicationDbContext.Users
                .AsNoTracking()
                .AnyAsync(
                u => u.Email.Equals(claim)
                && (u.Permissions.Any(p => p.Name.Equals(permission))
                || u.Roles.SelectMany(r => r.Permissions).Any(p => p.Name.Equals(permission))))
                .ConfigureAwait(false);
        }

        public async Task<Authorisation> GetAuthorisationAsync(string claim)
        {
            var user = await applicationDbContext.Users
                .AsNoTracking()
                .Include(u => u.Permissions)
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(u => u.Email.Equals(claim))
                .ConfigureAwait(false);

            var permissionSet = user.GetUserPermissionSet();

            return new Authorisation { User = claim, Permissions = permissionSet };
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    applicationDbContext.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

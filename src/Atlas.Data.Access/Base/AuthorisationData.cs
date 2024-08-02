using Atlas.Core.Exceptions;
using Atlas.Core.Models;
using Atlas.Data.Access.Context;
using Atlas.Data.Access.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Atlas.Data.Access.Base
{
    public abstract class AuthorisationData<T>(ApplicationDbContext applicationDbContext, ILogger<T> logger) 
        : DataBase<T>(applicationDbContext, logger), IAuthorisationData
    {
        public Authorisation? Authorisation { get; private set; }

        public async Task<Authorisation?> GetAuthorisationAsync(string? claim, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(claim))
            {
                return default;
            }

            User? user = await _applicationDbContext.Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(u => u.Email != null && u.Email.Equals(claim), cancellationToken)
                .ConfigureAwait(false) ?? throw new AtlasException($"{claim} unauthorized.", $"Claim {claim}");

            if (user.Email == null) throw new AtlasException($"Missing email.", $"Claim {claim}");

            SetUser(user.Email);

            Authorisation = new Authorisation { User = claim, Permissions = user.GetPermissions() };

            return Authorisation;
        }

        private void SetUser(string user)
        {
            _applicationDbContext.SetUser(user);
        }
    }
}

using Atlas.Core.Exceptions;
using Atlas.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Atlas.Data.Context
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

            try
            {
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
            catch (Exception ex)
            {
                if (ex.InnerException != null
                    && ex.InnerException.Message.StartsWith("Cannot open database"))
                {
                    throw new AtlasException(ex.InnerException.Message, ex);
                }

                throw new AtlasException(ex.Message, ex);
            }
        }

        private void SetUser(string user)
        {
            _applicationDbContext.SetUser(user);
        }
    }
}

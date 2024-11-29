using Atlas.Data.Access.Interfaces;
using Atlas.Data.Context;
using Microsoft.Extensions.Logging;

namespace Atlas.Data.Access.Data
{
    public class UserAuthorisationData(ApplicationDbContext applicationDbContext, ILogger<UserAuthorisationData> logger)
        : AuthorisationData<UserAuthorisationData>(applicationDbContext, logger), IUserAuthorisationData
    {
    }
}

using Atlas.Data.Access.EF.Context;
using Atlas.Data.Access.Interfaces;
using Microsoft.Extensions.Logging;

namespace Atlas.Data.Access.EF.Data
{
    public class UserAuthorisationData(ApplicationDbContext applicationDbContext, ILogger<UserAuthorisationData> logger)
        : AuthorisationData<UserAuthorisationData>(applicationDbContext, logger), IUserAuthorisationData
    {
    }
}

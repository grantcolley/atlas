using Atlas.Core.Models;

namespace Atlas.Data.Access.Interfaces
{
    public interface IClaimData : IAuthorisationData
    {
        IEnumerable<Module> GetDeveloperDatabaseClaim();
        Task<IEnumerable<Module>?> GetApplicationClaimsAsync(CancellationToken cancellationToken);
    }
}

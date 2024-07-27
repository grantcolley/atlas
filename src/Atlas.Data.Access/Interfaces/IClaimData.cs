using Atlas.Core.Models;

namespace Atlas.Data.Access.Interfaces
{
    public interface IClaimData : IAuthorisationData
    {
        Task<IEnumerable<Module>?> GetApplicationClaimsAsync(CancellationToken cancellationToken);
    }
}

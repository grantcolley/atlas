using Atlas.Core.Models;
using Atlas.Data.Context;

namespace Atlas.Data.Access.Interfaces
{
    public interface IClaimData : IAuthorisationData
    {
        Task<IEnumerable<Module>?> GetApplicationClaimsAsync(CancellationToken cancellationToken);
    }
}

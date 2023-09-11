using Atlas.Core.Models;

namespace Atlas.Data.Access.Interfaces
{
    public interface INavigationData : IAuthorisationData
    {
        Task<IEnumerable<Module>?> GetNavigationClaimsAsync(string claim, CancellationToken cancellationToken);
    }
}

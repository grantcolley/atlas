using Atlas.Core.Models;

namespace Atlas.Data.Access.Interfaces
{
    public interface IUserData : IAuthorisationData
    {
        Task SetThemePreferenceAsync(string theme, CancellationToken cancellationToken);
        Task<IEnumerable<Module>?> GetNavigationClaimsAsync(CancellationToken cancellationToken);
    }
}

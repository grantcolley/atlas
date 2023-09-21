using Atlas.Core.Models;

namespace Atlas.Requests.Interfaces
{
    public interface INavigationRequests
    {
        Task<IEnumerable<Module>?> GetClaimModulesAsync();
    }
}

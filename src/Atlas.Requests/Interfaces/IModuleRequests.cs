using Atlas.Core.Models;

namespace Atlas.Requests.Interfaces
{
    public interface IModuleRequests
    {
        Task<IEnumerable<Module>?> GetModulesAsync();
    }
}

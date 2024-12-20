using Atlas.Core.Models;

namespace Atlas.Requests.Interfaces
{
    public interface IClaimRequests
    {
        Task<IResponse<IEnumerable<Module>?>> GetClaimModulesAsync();
    }
}

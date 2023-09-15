using Atlas.Core.Models;

namespace Atlas.Requests.Interfaces
{
    public interface IComponentArgsRequests
    {
        Task<ComponentArgs?> GetComponentArgsAsync(string? componentCode);
    }
}

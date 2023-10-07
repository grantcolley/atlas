using Atlas.Core.Models;

namespace Atlas.Requests.Interfaces
{
    public interface IUserRequests
    {
        Task<string?> GetThemeAsync();
        Task SetThemeAsync(string theme);
        Task<IEnumerable<Module>?> GetClaimModulesAsync();
    }
}

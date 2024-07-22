using Atlas.Core.Models;
using System.Security.Claims;

namespace Atlas.Requests.Interfaces
{
    public interface IClaimRequests
    {
        Task<IEnumerable<Module>?> GetClaimModulesAsync();
        Task<Authorisation?> GetAuthorisationAsync(ClaimsPrincipal principal);
    }
}

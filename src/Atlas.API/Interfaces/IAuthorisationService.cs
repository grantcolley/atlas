using Atlas.Core.Models;

namespace Atlas.API.Interfaces
{
    public interface IAuthorisationService
    {
        Task<Authorisation?> GetAuthorisationAsync();
    }
}

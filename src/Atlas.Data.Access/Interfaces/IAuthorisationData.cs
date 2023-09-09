using Atlas.Core.Models;

namespace Atlas.Data.Access.Interfaces
{
    public interface IAuthorisationData
    {
        Task<Authorisation?> GetAuthorisationAsync(string user);
    }
}

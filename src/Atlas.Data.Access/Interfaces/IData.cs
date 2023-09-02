using Atlas.Core.Models;

namespace Atlas.Data.Access.Interfaces
{
    public interface IData : IDisposable
    {
        void SetUser(string user);
        Task<Authorisation> GetAuthorisationAsync(string claim);
        Task<bool> IsAuthorisedAsync(string claim, string permission);
    }
}

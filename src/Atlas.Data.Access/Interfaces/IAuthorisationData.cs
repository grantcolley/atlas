using Atlas.Core.Models;

namespace Atlas.Data.Access.Interfaces
{
    public interface IAuthorisationData
    {
        Authorisation? Authorisation { get; }
        Task<Authorisation?> GetAuthorisationAsync(string? user, CancellationToken cancellationToken);
    }
}

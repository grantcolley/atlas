using Atlas.Core.Models;

namespace Atlas.Data.Access.Interfaces
{
    public interface IComponentArgsData : IAuthorisationData
    {
        Task<ComponentArgs?> GetComponentArgsAsync(string? componentCode, CancellationToken cancellationToken);
    }
}

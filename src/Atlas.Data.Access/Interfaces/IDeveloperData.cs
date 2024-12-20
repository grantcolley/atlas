using Atlas.Core.Models;

namespace Atlas.Data.Access.Interfaces
{
    public interface IDeveloperData
    {
        Task<DatabaseStatus?> GetDatabaseStatusAsync(string? user, CancellationToken cancellationToken);
        Task<DatabaseStatus?> CreateDatabaseAsync(string? user, CancellationToken cancellationToken);
        Task<DatabaseStatus?> SeedDatabaseAsync(string? user, CancellationToken cancellationToken);
    }
}

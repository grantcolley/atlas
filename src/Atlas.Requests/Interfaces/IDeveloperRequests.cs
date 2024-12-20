using Atlas.Core.Models;

namespace Atlas.Requests.Interfaces
{
    public interface IDeveloperRequests
    {
        Task<IResponse<DatabaseStatus?>> GetDatabaseStatusAsync();
        Task<IResponse<DatabaseStatus?>> CreateDatabaseAsync();
        Task<IResponse<DatabaseStatus?>> SeedDatabaseAsync();
    }
}

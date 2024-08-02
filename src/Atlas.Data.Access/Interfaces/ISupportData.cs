using Atlas.Core.Models;

namespace Atlas.Data.Access.Interfaces
{
    public interface ISupportData : IAuthorisationData
    {
        Task<IEnumerable<Log>> GetLogsAsync(LogArgs logArgs, CancellationToken cancellationToken);
    }
}

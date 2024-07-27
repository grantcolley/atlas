using Atlas.Core.Models;

namespace Atlas.Requests.Interfaces
{
    public interface ILogRequests
    {
        Task LogAsync(Log log);
    }
}

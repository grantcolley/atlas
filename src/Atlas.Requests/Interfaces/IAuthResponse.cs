using Atlas.Core.Models;

namespace Atlas.Requests.Interfaces
{
    public interface IAuthResponse<T> : IResponse<T>
    {
        Authorisation? Authorisation { get; set; }
    }
}

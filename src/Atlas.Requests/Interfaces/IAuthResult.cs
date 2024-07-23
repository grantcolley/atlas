using Atlas.Core.Models;

namespace Atlas.Requests.Interfaces
{
    public interface IAuthResult<T> where T : new()
    {
        Authorisation Authorisation { get; }
        T Result { get; }
    }
}

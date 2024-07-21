using Atlas.Core.Models;

namespace Atlas.Core.Interfaces
{
    public interface IAPIResult<T> where T : new()
    {
        Authorisation Authorisation { get; }
        T Result { get; }
    }
}

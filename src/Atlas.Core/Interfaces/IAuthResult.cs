using Atlas.Core.Models;

namespace Atlas.Core.Interfaces
{
    public interface IAuthResult<T>
    {
        Authorisation? Authorisation { get; set; }
        T? Result { get; set; }
    }
}

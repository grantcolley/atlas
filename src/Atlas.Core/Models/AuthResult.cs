using Atlas.Core.Interfaces;

namespace Atlas.Core.Models
{
    public class AuthResult<T> : IAuthResult<T>
    {
        public Authorisation? Authorisation { get; set; }

        public T? Result { get; set; }
    }
}

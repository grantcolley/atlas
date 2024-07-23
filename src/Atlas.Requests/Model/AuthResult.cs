using Atlas.Core.Models;
using Atlas.Requests.Interfaces;

namespace Atlas.Requests.Model
{
    public class AuthResult<T> : IAuthResult<T> where T : new()
    {
        public Authorisation Authorisation { get; set; }

        public T Result => throw new NotImplementedException();
    }
}

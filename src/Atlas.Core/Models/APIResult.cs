using Atlas.Core.Interfaces;

namespace Atlas.Core.Models
{
    public class APIResult<T> : IAPIResult<T> where T : new()
    {
        public APIResult(Authorisation authorisation, T result) 
        {
            Authorisation = authorisation;
            Result = result;
        }

        public Authorisation Authorisation { get; }
        public T Result { get; }
    }
}

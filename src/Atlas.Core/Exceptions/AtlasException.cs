using System;

namespace Atlas.Core.Exceptions
{
    public class AtlasException : Exception
    {
        public AtlasException()
        {
        }

        public AtlasException(string message)
            : base(message)
        {
        }

        public AtlasException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

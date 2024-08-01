using System;

namespace Atlas.Core.Exceptions
{
    public class AtlasException : Exception
    {
        public AtlasException()
        {
        }

        public AtlasException(string message, string? context)
            : base(message)
        {
            Context = context;
        }

        public AtlasException(string message, Exception? inner = null, string? context = "")
            : base(message, inner)
        {
            Context = context;
        }

        public string? Context { get; }
    }
}

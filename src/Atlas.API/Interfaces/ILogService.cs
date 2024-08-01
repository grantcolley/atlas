using Atlas.Core.Exceptions;

namespace Atlas.API.Interfaces
{
    public interface ILogService
    {
        void Log(Enums.LogLevel logLevel, string? message, AtlasException? exception, string? user);
    }
}

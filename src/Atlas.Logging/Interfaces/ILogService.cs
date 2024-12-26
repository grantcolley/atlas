using Atlas.Core.Exceptions;
using Atlas.Logging.Enums;

namespace Atlas.Logging.Interfaces
{
    public interface ILogService
    {
        void Log(AtlasException? exception, string? user);
        void Log(LogLevel logLevel, string? message, string? user);
        void Log(LogLevel logLevel, AtlasException? exception, string? user);
        void Log(LogLevel logLevel, string? message, AtlasException? exception, string? user);
    }
}

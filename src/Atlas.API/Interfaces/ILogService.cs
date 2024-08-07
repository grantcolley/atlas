using Atlas.Core.Exceptions;

namespace Atlas.API.Interfaces
{
    public interface ILogService
    {
        void Log(AtlasException? exception, string? user);
        void Log(Enums.LogLevel logLevel, string? message, string? user);
        void Log(Enums.LogLevel logLevel, AtlasException? exception, string? user);
        void Log(Enums.LogLevel logLevel, string? message, AtlasException? exception, string? user);
    }
}

namespace Atlas.API.Interfaces
{
    public interface ILogService
    {
        void Log(Enums.LogLevel logLevel, string? message, string? user);
        void Log(Enums.LogLevel logLevel, string? message, Exception? exception, string? user);
    }
}

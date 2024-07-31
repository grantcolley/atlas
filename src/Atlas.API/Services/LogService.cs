using Atlas.API.Interfaces;
using Serilog.Context;

namespace Atlas.API.Services
{
    public class LogService : ILogService
    {
        private readonly ILogger<LogService> _logger;

        public LogService(ILogger<LogService> logger) 
        {
            ArgumentNullException.ThrowIfNull(nameof(logger));

            _logger = logger;
        }

        public void Log(Enums.LogLevel logLevel, string? message, string? user = "")
        {
            Log(logLevel, message, null, user);
        }

        public void Log(Enums.LogLevel logLevel, string? message, Exception? exception = null, string? user = "")
        {
            if (string.IsNullOrWhiteSpace(user))
            {
                Log(logLevel, message, exception);
            }
            else
            {
                using (LogContext.PushProperty("User", user))
                {
                    Log(logLevel, message, exception);
                }
            }
        }

        private void Log(Enums.LogLevel logLevel, string? message, Exception? exception)
        {
#pragma warning disable CA2254 // Template should be a static expression
            switch (logLevel)
            {
                case Enums.LogLevel.Information:
                    _logger.LogInformation(exception, message);
                    break;

                case Enums.LogLevel.Warning:
                    _logger.LogWarning(exception, message);
                    break;

                case Enums.LogLevel.Error:
                    _logger.LogError(exception, message);
                    break;
            }
#pragma warning restore CA2254 // Template should be a static expression
        }
    }
}

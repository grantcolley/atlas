using Atlas.Core.Exceptions;
using Atlas.Core.Logging.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Atlas.Core.Logging.Services
{
    public class LogService(ILogger<LogService> logger) : ILogService
    {
        private readonly ILogger<LogService> _logger = logger;

        public void Log(AtlasException? exception = null, string? user = "")
        {
            Log(Enums.LogLevel.Error, exception?.Message, exception, user);
        }

        public void Log(Enums.LogLevel logLevel, string? message, string? user = "")
        {
            Log(logLevel, message, null, user);
        }

        public void Log(Enums.LogLevel logLevel, AtlasException? exception = null, string? user = "")
        {
            Log(logLevel, exception?.Message, exception, user);
        }

        public void Log(Enums.LogLevel logLevel, string? message, AtlasException? exception = null, string? user = "")
        {
            using (LogContext.PushProperty("User", user))
            {
                using (LogContext.PushProperty("Context", exception?.Context))
                {
                    Log(logLevel, message, exception?.InnerException);
                }
            }
        }

        private void Log(Enums.LogLevel logLevel, string? message, Exception? exception)
        {
            if (string.IsNullOrEmpty(message)
                && exception != null)
            {
                message = exception.Message;
            }

#pragma warning disable IDE0079 // Remove unnecessary suppression
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
#pragma warning restore IDE0079 // Remove unnecessary suppression
        }
    }
}

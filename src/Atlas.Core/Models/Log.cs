using Atlas.Core.Enums;

namespace Atlas.Core.Models
{
    public class Log
    {
        public LogLevel Level { get; set; }
        public string? Message { get; set; }
        public string? Details { get; set; }
    }
}

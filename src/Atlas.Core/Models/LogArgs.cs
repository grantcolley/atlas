using System;

namespace Atlas.Core.Models
{
    public class LogArgs
    {
        public string? User { get; set; }
        public string? Message { get; set; }
        public string? Context { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}

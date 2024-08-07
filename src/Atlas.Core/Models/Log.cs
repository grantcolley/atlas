using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlas.Core.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public string? MessageTemplate { get; set; }
        public string? Level { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string? Exception { get; set; }
        public string? Properties { get; set; }

        [StringLength(450)]
        public string? Context { get; set; }

        [StringLength(450)]
        public string? User { get; set; }

        [NotMapped]
        public string? LogIcon
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(Level)) return "Circle";
                else if (Level.Equals("Error")) return "DismissCircle";
                else if (Level.Equals("Warning")) return "Warning";
                else if (Level.Equals("Information")) return "Info";
                else return "Circle";
            } 
        }
    }
}

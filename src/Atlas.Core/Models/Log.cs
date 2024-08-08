using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

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
        [JsonIgnore]
        public string? Icon
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

        public List<LogProperty> GetLogProperties()
        {
            if (string.IsNullOrWhiteSpace(Properties)) return [];

            var serializer = new XmlSerializer(typeof(LogProperties));

            using TextReader reader = new StringReader(Properties);
            LogProperties? logProperties = (LogProperties?)serializer.Deserialize(reader);

            if (logProperties == null || logProperties.Properties == null) return [];

            return logProperties.Properties;
        }
    }
}

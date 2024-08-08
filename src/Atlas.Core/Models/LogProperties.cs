using System.Collections.Generic;
using System.Xml.Serialization;

namespace Atlas.Core.Models
{
    [XmlRoot("properties")]
    public class LogProperties
    {
        public LogProperties() 
        {
            Properties = [];
        }

        [XmlElement("property")]
        public List<LogProperty>? Properties { get; set; }
    }
}

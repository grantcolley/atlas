using System.Xml.Serialization;

namespace Atlas.Core.Models
{
    [XmlRoot("property")]
    public class LogProperty
    {
        [XmlAttribute("key")]
        public string? Key { get; set; }

        [XmlText]
        public string? Value { get; set; }
    }
}

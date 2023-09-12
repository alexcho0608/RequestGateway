using System.Xml.Serialization;

namespace DTOs.Models.XmlAPI
{
    public class Enter
    {
        [XmlAttribute("session")]
        public string Session { get; set; }

        [XmlElement("timestamp")]
        public long Timestamp { get; set; }

        [XmlElement("player")]
        public int Player { get; set; }
    }
}

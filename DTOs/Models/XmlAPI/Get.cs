using System.Xml.Serialization;

namespace DTOs.Models.XmlAPI
{
    public class Get
    {
        [XmlAttribute("session")]
        public string Session { get; set; }
    }
}

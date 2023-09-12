namespace DTOs.Models.XmlAPI
{
    using System.Xml.Serialization;
    [XmlRoot(ElementName = "command")]
    public class Command
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("enter")]
        public Enter? Enter { get; set; }

        [XmlElement("get")]
        public Get? Get { get; set; }
    }
}

using System.Xml.Linq;

namespace ModsDeApi.Services.Board
{
    public class Board
    {
        public int Id { get; }
        public int CategoryId { get; }
        public string Name { get; }
        public string Description { get; }
        public int Threads { get; }
        public int Replies { get; }
        

        protected internal Board(XElement xml)
        {
            Id = XmlHelper.GetAttributeValueAsInt(xml, Constants.XmlAttributeId);
            CategoryId = XmlHelper.GetChildAttributeValueAsInt(xml, Constants.XmlTagInCategory, Constants.XmlAttributeId);
            Name = XmlHelper.GetChildValue(xml, Constants.XmlTagName);
            Description = XmlHelper.GetChildValue(xml, Constants.XmlTagDescription);
            Threads = XmlHelper.GetChildAttributeValueAsInt(xml, Constants.XmlTagNumberOfThreads, Constants.XmlAttributeValue);
            Replies = XmlHelper.GetChildAttributeValueAsInt(xml, Constants.XmlTagNumberOfReplies, Constants.XmlAttributeValue);
        }

        public override string ToString()
        {
            return $"{Name} ({Id})";
        }
    }
}

using System.Xml.Linq;

namespace ModsDeApi.Services.Thread
{
    public class FirstPost : LastPost
    {
        public int IconId { get; }

        protected internal FirstPost(XElement xml) : base(xml)
        {
            IconId = XmlHelper.GetChildAttributeValueAsInt(xml, Constants.XmlTagIcon, Constants.XmlAttributeId);
        }
    }
}

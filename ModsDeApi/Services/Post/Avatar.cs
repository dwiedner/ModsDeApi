using System;
using System.Xml.Linq;

namespace ModsDeApi.Services.Post
{
    public class Avatar
    {
        public int Id { get; }
        public string FilePath { get; }

        protected internal Avatar(XElement xml)
        {
            if (xml == null)
                throw new ArgumentNullException(nameof(xml));

            if (!xml.Name.EqualsIgnoreCase(Constants.XmlTagAvatar))
                throw new ArgumentException($"Invalid XML tag [{xml.Name}], expected [{Constants.XmlTagAvatar}]");

            Id = XmlHelper.GetAttributeValueAsInt(xml, Constants.XmlAttributeId);
            FilePath = xml.Value;
        }

        public override string ToString()
        {
            return $"{FilePath} ({Id})";
        }
    }
}

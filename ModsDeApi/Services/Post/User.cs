using System;
using System.Xml.Linq;

namespace ModsDeApi.Services.Post
{
    public class User
    {
        public int Id { get; }
        public string Name { get; }

        public User(XElement xml)
        {
            if (xml == null)
                throw new ArgumentNullException(nameof(xml));

            if (!xml.Name.EqualsIgnoreCase(Constants.XmlTagUser))
                throw new ArgumentException($"Invalid XML tag [{xml.Name}], expected [{Constants.XmlTagUser}]");

            Id = XmlHelper.GetAttributeValueAsInt(xml, Constants.XmlAttributeId);
            Name = xml.Value;
        }

        public override string ToString()
        {
            return $"{Name} ({Id})";
        }
    }
}

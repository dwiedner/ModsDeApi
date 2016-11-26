using System;
using System.Linq;
using System.Xml.Linq;
using ModsDeApi.Services.Post;

namespace ModsDeApi.Services.Thread
{
    public class LastPost
    {
        public User User { get; }
        public DateTime Date { get; }

        protected internal LastPost(XElement xml)
        {
            if (!xml.Name.EqualsIgnoreCase(Constants.XmlTagPost))
                throw new Exception($"Invalid XML element [{xml.Name}]");

            Date = ReadDate(xml);

            var userElement = xml.Elements().FirstOrDefault(x => x.Name.EqualsIgnoreCase(Constants.XmlTagUser));
            if (userElement != null)
                User = new User(userElement);
        }

        private DateTime ReadDate(XElement xml)
        {
            var timestamp = XmlHelper.GetChildAttributeValueAsInt(xml, Constants.XmlTagDate, Constants.XmlAttributeTimestamp);
            return timestamp.ToDateTime();
        }

        public override string ToString()
        {
            return $"{Date}, {User}";
        }
    }
}

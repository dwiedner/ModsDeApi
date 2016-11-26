using System;
using System.Linq;
using System.Xml.Linq;

namespace ModsDeApi.Services.Post
{
    public class Message
    {
        public string Title { get; }
        public string Content { get; }
        public int EditCount { get; }
        public User LastEditor { get; }
        public DateTime LastEdit { get; }

        protected internal Message(XElement xml)
        {
            if (xml == null)
                throw new ArgumentNullException(nameof(xml));

            if (!xml.Name.EqualsIgnoreCase(Constants.XmlTagMessage))
                throw new ArgumentException($"Invalid XML tag [{xml.Name}], expected [{Constants.XmlTagMessage}]");

            Title = XmlHelper.GetChildValue(xml, Constants.XmlTagTitle);
            Content = XmlHelper.GetChildValue(xml, Constants.XmlTagContent);
            EditCount = XmlHelper.GetChildAttributeValueAsInt(xml, Constants.XmlTagEdited, Constants.XmlAttributeCount);

            var editedElement = xml.Elements().FirstOrDefault(x => x.Name.EqualsIgnoreCase(Constants.XmlTagEdited));
            var lastEdit = editedElement?.Elements().FirstOrDefault(x => x.Name.EqualsIgnoreCase(Constants.XmlTagLastedit));
            if (lastEdit != null)
            {
                var userElement = lastEdit.Elements().FirstOrDefault(x => x.Name.EqualsIgnoreCase(Constants.XmlTagUser));
                if (userElement != null)
                    LastEditor = new User(userElement);

                var dateElement = lastEdit.Elements().FirstOrDefault(x => x.Name.EqualsIgnoreCase(Constants.XmlTagDate));
                if (dateElement != null)
                {
                    var timestamp = XmlHelper.GetAttributeValueAsInt(dateElement, Constants.XmlAttributeTimestamp);
                    LastEdit = timestamp.ToDateTime();
                }
            }
        }
    }
}

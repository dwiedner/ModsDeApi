using System;
using System.Linq;
using System.Xml.Linq;

namespace ModsDeApi.Services.Post
{
    public class Post
    {
        public int Id { get; }
        public int ThreadId { get; }
        public User User { get; }
        public DateTime Date { get; }
        public Message Message { get; }
        public Avatar Avatar { get; }

        protected internal Post(int threadId, XElement xml)
        {
            if (xml == null)
                throw new ArgumentNullException(nameof(xml));

            if (!xml.Name.EqualsIgnoreCase(Constants.XmlTagPost))
                throw new ArgumentException($"Invalid XML tag [{xml.Name}], expected [{Constants.XmlTagPost}]");

            ThreadId = threadId;
            Id = XmlHelper.GetAttributeValueAsInt(xml, Constants.XmlAttributeId);

            var timestamp = XmlHelper.GetChildAttributeValueAsInt(xml, Constants.XmlTagDate, Constants.XmlAttributeTimestamp);
            Date = timestamp.ToDateTime();

            var userElement = xml.Elements().FirstOrDefault(x => x.Name.EqualsIgnoreCase(Constants.XmlTagUser));
            if (userElement != null)
                User = new User(userElement);

            var messageElement = xml.Elements().FirstOrDefault(x => x.Name.EqualsIgnoreCase(Constants.XmlTagMessage));
            if (messageElement != null)
                Message = new Message(messageElement);

            var avatarElement = xml.Elements().FirstOrDefault(x => x.Name.EqualsIgnoreCase(Constants.XmlTagAvatar));
            if (avatarElement != null)
                Avatar = new Avatar(avatarElement);
        }
    }
}

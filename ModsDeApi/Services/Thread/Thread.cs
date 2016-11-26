using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ModsDeApi.Services.Thread
{
    public class Thread
    {
        [Flags]
        public enum ThreadFlags
        {
            None = 0,
            Closed = 1,
            Sticky = 2,
            Important = 4,
            Announcement = 8,
            Global = 16,
            All = ~0
        }

        private static readonly IDictionary<string, ThreadFlags> ConstantsDictionary = new Dictionary
            <string, ThreadFlags>
        {
            {Constants.XmlTagIsAnnouncement, ThreadFlags.Announcement},
            {Constants.XmlTagIsClosed, ThreadFlags.Closed},
            {Constants.XmlTagIsGlobal, ThreadFlags.Global},
            {Constants.XmlTagIsImportant, ThreadFlags.Important},
            {Constants.XmlTagIsSticky, ThreadFlags.Sticky}
        };

        public int Id { get; }
        public int BoardId { get; }
        public string Title { get; }
        public string Subtitle { get; }
        public ThreadFlags Flags { get; }
        public int Replies { get; }
        public int Hits { get; }
        public int Pages { get; }
        public FirstPost FirstPost { get; }
        public LastPost LastPost { get; }

        public Thread(XElement xml)
        {
            Id = XmlHelper.GetAttributeValueAsInt(xml, Constants.XmlAttributeId);
            BoardId = XmlHelper.GetChildAttributeValueAsInt(xml, Constants.XmlTagInBoard, Constants.XmlAttributeId);
            Title = XmlHelper.GetChildValue(xml, Constants.XmlTagTitle);
            Subtitle = XmlHelper.GetChildValue(xml, Constants.XmlTagSubtitle);
            Flags = ReadFlags(xml);
            Replies = XmlHelper.GetChildAttributeValueAsInt(xml, Constants.XmlTagNumberOfReplies, Constants.XmlAttributeValue);
            Hits = XmlHelper.GetChildAttributeValueAsInt(xml, Constants.XmlTagNumberOfHits, Constants.XmlAttributeValue);
            Pages = XmlHelper.GetChildAttributeValueAsInt(xml, Constants.XmlTagNumberOfPages, Constants.XmlAttributeValue);

            FirstPost = ReadFirstPost(xml);
            LastPost = ReadLastPost(xml);
        }

        private ThreadFlags ReadFlags(XElement xml)
        {
            var flagsElement = xml.Elements().FirstOrDefault(x => x.Name.EqualsIgnoreCase(Constants.XmlTagFlags));
            if (flagsElement == null)
                return ThreadFlags.None;

            

            var threadFlags = ThreadFlags.None;
            foreach (var flagElement in flagsElement.Elements())
            {
                foreach (var constants in ConstantsDictionary)
                {
                    if (flagElement.Name.EqualsIgnoreCase(constants.Key))
                    {
                        var flagValue = XmlHelper.GetAttributeValueAsInt(flagElement, Constants.XmlAttributeValue);
                        if (flagValue == 1)
                            threadFlags |= constants.Value;
                    }
                }
            }

            return threadFlags;
        }

        private XElement GetPostElement(XElement xml)
        {
            var postElement = xml.Elements().FirstOrDefault(x => x.Name.EqualsIgnoreCase(Constants.XmlTagPost));
            if (postElement == null)
                throw new Exception($"[{xml.Name} element doesn't contain a [{Constants.XmlTagPost}] element");

            return postElement;
        }
        private FirstPost ReadFirstPost(XElement xml)
        {
            var firstPostElement = xml.Elements().FirstOrDefault(x => x.Name.EqualsIgnoreCase(Constants.XmlTagFirstpost));
            if (firstPostElement == null)
                return null;

            var postElement = GetPostElement(firstPostElement);

            return new FirstPost(postElement);
        }

        private LastPost ReadLastPost(XElement xml)
        {
            var lastPostElement = xml.Elements().FirstOrDefault(x => x.Name.EqualsIgnoreCase(Constants.XmlTagLastpost));
            if (lastPostElement == null)
                return null;

            var postElement = GetPostElement(lastPostElement);

            return new LastPost(postElement);
        }

        public override string ToString()
        {
            return $"{Title} ({Id})";
        }
    }
}

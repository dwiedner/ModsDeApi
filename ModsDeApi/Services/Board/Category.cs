using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace ModsDeApi.Services.Board
{
    public class Category
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }

        public IReadOnlyCollection<Board> Boards { get; } 

        protected internal Category(XElement xml)
        {
            if (xml == null)
                throw new ArgumentNullException(nameof(xml));

            if (!xml.Name.EqualsIgnoreCase(Constants.XmlTagCategory))
                throw new ArgumentException($"Invalid XML element [{xml.Name}]");


            Id = XmlHelper.GetAttributeValueAsInt(xml, Constants.XmlAttributeId);
            Name = XmlHelper.GetChildValue(xml, Constants.XmlTagName);
            Description = XmlHelper.GetChildValue(xml, Constants.XmlTagDescription);
            Boards = ReadBoards(xml);
        }

        private IReadOnlyCollection<Board> ReadBoards(XElement xml)
        {
            var boardsElement = xml.Elements().FirstOrDefault(x => x.Name.EqualsIgnoreCase(Constants.XmlTagBoards));
            if (boardsElement == null)
                return new ReadOnlyCollection<Board>(new List<Board>());

            var boards = boardsElement.Elements().Where(x => x.Name.EqualsIgnoreCase(Constants.XmlTagBoard)).Select(x => new Board(x));
            return new ReadOnlyCollection<Board>(boards.ToList());
        }

        public override string ToString()
        {
            return $"{Name} ({Id})";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ModsDeApi.Services.Board
{
    public class BoardService : IBoardService
    {
        private const string BoardsUrl = "http://forum.mods.de/bb/xml/boards.php";
        private const string SingleBoardUrl = "http://forum.mods.de/bb/xml/board.php?BID={0}";

        public static IBoardService Instance { get; } = new BoardService();

        private BoardService() { }

        public async Task<IReadOnlyCollection<Category>> GetCategories()
        {
            var xDoc = await XmlHelper.LoadUrl(BoardsUrl);
            if (xDoc == null)
                throw new Exception("No XML document received");

            var root = xDoc.Root;
            if (root == null)
                throw new Exception("No document element found");

            if (!root.Name.EqualsIgnoreCase(Constants.XmlTagCategories))
                throw new Exception($"Invalid document element [{root.Name}]");
            
            var categories = root.Elements().Where(x => x.Name.EqualsIgnoreCase(Constants.XmlTagCategory)).Select(x => new Category(x));
            return new ReadOnlyCollection<Category>(categories.ToList());
        }

        public async Task<IReadOnlyCollection<Board>> GetBoards()
        {
            var categories = await GetCategories();
            return new ReadOnlyCollection<Board>(categories.SelectMany(x => x.Boards).ToList());
        }

        public async Task<Board> GetBoard(int boardId)
        {
            var url = string.Format(SingleBoardUrl, boardId);
            var xDoc = await XmlHelper.LoadUrl(url);
            if (xDoc == null)
                throw new Exception("No XML document received");

            var root = xDoc.Root;
            if (root == null)
                throw new Exception("No document element found");

            if (!root.Name.EqualsIgnoreCase(Constants.XmlTagBoard))
                throw new Exception($"Invalid document element [{root.Name}]");

            return new Board(root);
        }
    }
}

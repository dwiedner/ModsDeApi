using ModsDeApi.Services.Board;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ModsDeApi.Services.Thread
{
    public class ThreadService : IThreadService
    {
        private const string ThreadsUrl = "http://forum.mods.de/bb/xml/board.php?BID={0}&page={1}";
        private const string SingleThreadUrl = "http://forum.mods.de/bb/xml/thread.php?TID={0}";

        public static IThreadService Instance { get; } = new ThreadService();

        private ThreadService() { }
        
        public async Task<IReadOnlyCollection<Thread>> GetThreads(int boardId, int page = 1)
        {
            var url = string.Format(ThreadsUrl, boardId, page);
            var xDoc = await XmlHelper.LoadUrl(url);
            if (xDoc == null)
                throw new Exception("No XML document received");

            var root = xDoc.Root;
            if (root == null)
                throw new Exception("No document element found");

            if (!root.Name.EqualsIgnoreCase(Constants.XmlTagBoard))
                throw new Exception($"Invalid document element [{root.Name}]");

            var xmlBoardId = XmlHelper.GetAttributeValueAsInt(root, Constants.XmlAttributeId);
            if (boardId != xmlBoardId)
                throw new Exception($"Incorrect board: requested [{boardId}], received [{xmlBoardId}]");

            var threadsElement = root.Elements().FirstOrDefault(x => x.Name.EqualsIgnoreCase(Constants.XmlTagThreads));
            if (threadsElement == null)
                return new ReadOnlyCollection<Thread>(new List<Thread>());

            var threads = threadsElement.Elements()
                .Where(x => x.Name.EqualsIgnoreCase(Constants.XmlTagThread))
                .Select(x => new Thread(x));

            return new ReadOnlyCollection<Thread>(threads.ToList());
        }

        public async Task<Thread> GetThread(int threadId)
        {
            var url = string.Format(SingleThreadUrl, threadId);
            var xDoc = await XmlHelper.LoadUrl(url);
            if (xDoc == null)
                throw new Exception("No XML document received");

            var root = xDoc.Root;
            if (root == null)
                throw new Exception("No document element found");

            if (root.Name.EqualsIgnoreCase(Constants.XmlTagInvalidThread))
                throw new InvalidThreadIdException(threadId);

            if (!root.Name.EqualsIgnoreCase(Constants.XmlTagThread))
                throw new Exception($"Invalid document element [{root.Name}]");

            return new Thread(root);
        }

        public async Task<IReadOnlyCollection<Thread>> GetAllThreads(int boardId)
        {
            var board = await BoardService.Instance.GetBoard(boardId);
            if (board == null)
                return null;

            var allThreads = new List<Thread>();
            Parallel.For(1, (int) Math.Ceiling((double) (board.Threads/30)) + 1, page => {
                var threads = GetThreads(boardId, page).Result;

                lock (allThreads)
                    allThreads.AddRange(threads);
            });

            return allThreads;
        }
    }
}

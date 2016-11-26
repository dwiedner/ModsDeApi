using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModsDeApi.Services.Board;
using ModsDeApi.Services.Thread;
using System.Linq;
using System.Threading.Tasks;
using static ModsDeApi.Services.Thread.Thread;

namespace UnitTests
{
    [TestClass]
    public class ThreadServiceTest
    {
        [TestMethod]
        public async Task GetThreadsTest()
        {
            var service = ThreadService.Instance;

            var threads = await service.GetThreads(TestData.BoardId);
            
            Assert.IsNotNull(threads);
            Assert.IsTrue(threads.Any(x => x.Flags.HasFlag(Thread.ThreadFlags.Global)));
            Assert.IsTrue(threads.Any(x => x.Flags.HasFlag(Thread.ThreadFlags.Sticky)));


            threads = await service.GetThreads(TestData.BoardId, 2);
            
            Assert.IsNotNull(threads);
            Assert.IsFalse(threads.Any(x => x.Flags.HasFlag(Thread.ThreadFlags.Global)));
            Assert.IsFalse(threads.Any(x => x.Flags.HasFlag(Thread.ThreadFlags.Sticky)));
        }

        [TestMethod]
        public async Task GetThreadTest()
        {
            var service = ThreadService.Instance;

            var thread = await service.GetThread(TestData.ThreadId);

            Assert.IsNotNull(thread);
       }

        [TestMethod]
        [ExpectedException(typeof(InvalidThreadIdException))]
        public async Task InvalidThreadIdTest()
        {
            var service = ThreadService.Instance;

            var thread = await service.GetThread(0);
        }

        [TestMethod]
        public async Task GetAllThreadsTest()
        {
            var service = ThreadService.Instance;

            var board = await BoardService.Instance.GetBoard(TestData.BoardId);
            var threads = await service.GetAllThreads(TestData.BoardId);

            Assert.IsNotNull(threads);
            Assert.AreEqual(board.Threads, threads.Count);
            Assert.IsTrue(threads.Where(x => !x.Flags.HasFlag(ThreadFlags.Global)).All(x => x.BoardId == TestData.BoardId));
            Assert.IsTrue(threads.All(x => x != null));
        }
    }
}

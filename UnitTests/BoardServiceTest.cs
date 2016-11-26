using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModsDeApi.Services.Board;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class BoardServiceTest
    {
        [TestMethod]
        public async Task GetCategoriesTest()
        {
            var service = BoardService.Instance;

            var categories = await service.GetCategories();
            
            Assert.AreEqual(TestData.NumberOfCategories, categories.Count);

            foreach (var category in categories)
            {
                Assert.IsNotNull(category.Boards);

                int numberOfBoards;
                Assert.IsTrue(TestData.BoardsPerCategory.TryGetValue(category.Id, out numberOfBoards));
                Assert.AreEqual(numberOfBoards, category.Boards.Count);
            }
        }

        [TestMethod]
        public async Task GetBoardsTest()
        {
            var service = BoardService.Instance;

            var boards = await service.GetBoards();

            int totalNumberOfBoards = TestData.BoardsPerCategory.Select(x => x.Value).Sum();
            Assert.AreEqual(totalNumberOfBoards, boards.Count);
        }
    }
}

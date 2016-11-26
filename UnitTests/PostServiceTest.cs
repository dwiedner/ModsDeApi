using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModsDeApi.Services.Post;
using ModsDeApi.Services.Thread;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class PostServiceTest
    {
        [TestMethod]
        public async Task GetPostsTest()
        {
            var service = PostService.Instance;

            var postsFirstPage = await service.GetPosts(TestData.ThreadId);

            Assert.IsNotNull(postsFirstPage);

            var postsSecondPage = await service.GetPosts(TestData.ThreadId, 2);

            Assert.IsNotNull(postsSecondPage);

            Assert.IsTrue(postsFirstPage.All(x => postsSecondPage.All(y => x.Id != y.Id)));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidThreadIdException))]
        public async Task InvalidThreadIdTest()
        {
            var service = PostService.Instance;

            var posts = await service.GetPosts(0);
        }

        [TestMethod]
        public async Task GetAllPostsTest()
        {
            var service = PostService.Instance;

            var posts = await service.GetAllPosts(TestData.ThreadId);

            Assert.IsNotNull(posts);
            Assert.AreEqual(1475, posts.Count);
            Assert.IsTrue(posts.All(x => x.ThreadId == TestData.ThreadId));
            Assert.IsTrue(posts.All(x => x != null));
        }
    }
}

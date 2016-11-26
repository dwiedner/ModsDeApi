using ModsDeApi.Services.Thread;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ModsDeApi.Services.Post
{
    public class PostService
    {
        private const string PostsUrl = "http://forum.mods.de/bb/xml/thread.php?TID={0}&page={1}";

        public static PostService Instance { get; } = new PostService();

        private PostService() { }

        public async Task<IReadOnlyCollection<Post>> GetPosts(int threadId, int page = 1)
        {
            var url = string.Format(PostsUrl, threadId, page);
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

            var xmlThreadId = XmlHelper.GetAttributeValueAsInt(root, Constants.XmlAttributeId);
            if (threadId != xmlThreadId)
                throw new Exception($"Incorrect board: requested [{threadId}], received [{xmlThreadId}]");

            var postsElement = root.Elements().FirstOrDefault(x => x.Name.EqualsIgnoreCase(Constants.XmlTagPosts));
            if (postsElement == null)
                return new ReadOnlyCollection<Post>(new List<Post>());

            var posts = postsElement.Elements()
                .Where(x => x.Name.EqualsIgnoreCase(Constants.XmlTagPost))
                .Select(x => new Post(threadId, x));

            return new ReadOnlyCollection<Post>(posts.ToList());
        }

        public async Task<IReadOnlyCollection<Post>> GetAllPosts(int threadId)
        {
            var thread = await ThreadService.Instance.GetThread(threadId);
            if (thread == null)
                return null;

            var allPosts = new List<Post>();
            Parallel.For(1, thread.Pages + 1, page => {
                var posts = GetPosts(threadId, page).Result;

                lock (allPosts)
                    allPosts.AddRange(posts);
            });

            return allPosts;
        }
    }
}

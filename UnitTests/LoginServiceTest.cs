using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModsDeApi.Services.Login;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class LoginServiceTest
    {
        [TestMethod]
        public async Task LoginTest()
        {
            var service = LoginService.Instance;

            var cookieCollection = await service.Login(TestData.Username, TestData.Password);
            Assert.IsNotNull(cookieCollection);

            Assert.IsTrue(cookieCollection.Cast<Cookie>()
                .Any(x => x.Name.Equals("MDESID", StringComparison.InvariantCultureIgnoreCase)));
        }
    }
}

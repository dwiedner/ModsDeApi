using System;
using System.Net;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ModsDeApi.Services.Login
{
    public class LoginService
    {
        private const string LoginUrl = "http://login.mods.de/";
        private const string LoginSsoForumUrl = "http://forum.mods.de/SSO.php?UID={0}&login={1}&lifetime={2}";
        private const string LoginSsoMyUrl = "http://my.mods.de/p/SSO.php?UID={0}&login={1}&lifetime={2}";

        private const string SsoPattern = @"http://(?:forum|my).mods.de/(?:p/)?SSO.php\?UID=([0-9]+)&login=([a-fA-F0-9]{20}[^&]+)&lifetime=([0-9]+)";

        public static LoginService Instance { get; } = new LoginService();

        private LoginService() { }

        public async Task<CookieCollection> Login(string userName, string password, int loginLifetime = 31536000)
        {
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                var response = await client.UploadStringTaskAsync(LoginUrl, "POST", $"login_username={userName}&login_password={password}&login_lifetime={loginLifetime}");

                var loginResult = ParseResponse(response);

                var forumSso =
                    await client.DownloadStringTaskAsync(string.Format(LoginSsoForumUrl, loginResult.UserId,
                        loginResult.LoginId, loginResult.Lifetime));

                if (!string.IsNullOrEmpty(forumSso))
                    throw new Exception($"Login failed, received SSO response [{forumSso}]");

                var mySso = await client.DownloadStringTaskAsync(string.Format(LoginSsoMyUrl, loginResult.UserId,
                        loginResult.LoginId, loginResult.Lifetime));

                if (!string.IsNullOrEmpty(mySso))
                    throw new Exception($"Login failed, received SSO response [{mySso}]");

                var cookieString = client.ResponseHeaders[HttpResponseHeader.SetCookie];
                return CookieHelper.GetAllCookiesFromHeader(cookieString, client.BaseAddress);
            }
        }

        public async Task<CookieCollection> Login(string userName, SecureString password, int loginLifetime = 31536000)
        {
            return await Login(userName, password.ToUnsecureString(), loginLifetime);
        }

        private LoginResponse ParseResponse(string loginResponse)
        {
            if (loginResponse == null)
                throw new ArgumentNullException(nameof(loginResponse));

            var match = Regex.Match(loginResponse, SsoPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (match == null)
                throw new ArgumentException($"Invalid login response string: [{loginResponse}]");

            int userId;
            int.TryParse(match.Groups[1].Value, out userId);

            int lifetime;
            int.TryParse(match.Groups[3].Value, out lifetime);

            var loginId = match.Groups[2].Value;

            return new LoginResponse
            {
                LoginId = loginId,
                UserId = userId,
                Lifetime = lifetime
            };
        }
    }
}

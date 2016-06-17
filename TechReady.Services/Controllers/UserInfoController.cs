using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using LinqToTwitter;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TechReady.Services.Controllers
{
    public class UserInfoController : ApiController
    {
        public ApiServices Services { get; set; }

        [AuthorizeLevel(AuthorizationLevel.User)]
        public async Task<JObject> GetUserInfo()
        {
            ServiceUser user = this.User as ServiceUser;
            if (user == null)
            {
                throw new InvalidOperationException("This can only be called by authenticated clients");
            }

            var identities = await user.GetIdentitiesAsync();
            var result = new JObject();
            var fb = identities.OfType<FacebookCredentials>().FirstOrDefault();
            if (fb != null)
            {
                var accessToken = fb.AccessToken;
                result.Add("facebook", await GetProviderInfo("https://graph.facebook.com/me?fields=id,name,email&access_token=" + accessToken));
            }

            var ms = identities.OfType<MicrosoftAccountCredentials>().FirstOrDefault();
            if (ms != null)
            {
                var accessToken = ms.AccessToken;
                result.Add("microsoft", await GetProviderInfo("https://apis.live.net/v5.0/me/?method=GET&access_token=" + accessToken));
            }

            var tw = identities.OfType<TwitterCredentials>().FirstOrDefault();
            if (tw != null)
            {
                var accessToken = tw.AccessToken;
                var accessTokenSecret = tw.AccessTokenSecret;
                var consumerKey = string.Empty;
                var consumerSecret = string.Empty;
                Services.Settings.TryGetValue("Twitter_ConsumerKey", out consumerKey);
                Services.Settings.TryGetValue("Twitter_ConsumerSecret", out consumerSecret);
              
                var auth = new SingleUserAuthorizer()
                {
                    CredentialStore = new SingleUserInMemoryCredentialStore()
                    {
                        ConsumerKey = consumerKey,
                        ConsumerSecret = consumerSecret,
                        AccessToken = accessToken,
                        AccessTokenSecret = accessTokenSecret
                    }
                };

                var twitterCtx = new TwitterContext(auth);

                ulong id = Convert.ToUInt64(user.Id.Replace("Twitter:", string.Empty));

                var x = (from c in twitterCtx.User
                    where c.Type == UserType.Show && c.UserID == id
                    select new {c.UserID, c.Name, c.ScreenName, c.Location}).FirstOrDefault();
                if (x != null)
                {
                    result.Add("twitter", JToken.FromObject(x));
                }
            }

            return result;
        }

        private async Task<JToken> GetProviderInfo(string url)
        {
            var c = new HttpClient();
            var resp = await c.GetAsync(url);
            resp.EnsureSuccessStatusCode();
            return JToken.Parse(await resp.Content.ReadAsStringAsync());
        }


       
    }
}

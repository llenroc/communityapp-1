using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using TechreadyForms.Helpers.AuthenticationHelper;
using TechReady.NavigationParameters;
using Xamarin.Forms;
using System.Threading.Tasks;
using Org.Json;
using TechReady.Helpers;
using TechReady.Helpers.MessageHelper;


[assembly: Xamarin.Forms.Dependency(typeof(TechreadyForms.Droid.Helpers.AuthenticationHelper.AuthenticationHelper))]


namespace TechreadyForms.Droid.Helpers.AuthenticationHelper
{
    public class AuthenticationHelper : IAuthenticationHelper
    {

        public AuthenticationHelper()
        {
            //change Azure Mobile Service Connection details
            MobileService = new MobileServiceClient(
                "https://indiatech.azure-mobile.net/",
                "NtCVhrhoQLLRblkwOMyQQGHOthBBjA60"
                );

        }

        private MobileServiceUser user;

        public MobileServiceClient MobileService;
        public async Task<UserRegistrationPageNavigationParameter> Authenticate(MobileServiceAuthenticationProvider provider)
        {
          

            string username = "", email = "", authProvider = "", authProviderUserId = "";
            try
            {

                // Change 'MobileService' to the name of your MobileServiceClient instance.
                // Sign-in using Facebook authentication.
                user = await this.MobileService
                    .LoginAsync(Forms.Context, provider);

                if (user != null)
                {
                    var userInfo = await this.MobileService.InvokeApiAsync("UserInfo", HttpMethod.Get, null);
                    //if (userInfo != null)
                    //{
                    //    Frame.Navigate(typeof(UserRegisterationPage))
                    //}

                    if (userInfo != null)
                    {
                        switch (provider)
                        {
                            case MobileServiceAuthenticationProvider.Facebook:
                                username = (string) userInfo["facebook"]["name"] ?? "";
                                email = (string) userInfo["facebook"]["email"] ?? "";
                                authProvider = "facebook";
                                authProviderUserId = user.UserId;
                                break;
                            case MobileServiceAuthenticationProvider.MicrosoftAccount:
                                username = (string) userInfo["microsoft"]["name"] ?? "";
                                email = (string) userInfo["microsoft"]["emails"]["account"] ?? "";
                                authProvider = "microsoft";
                                authProviderUserId = user.UserId;
                                break;
                            case MobileServiceAuthenticationProvider.Twitter:
                                username = (string) userInfo["twitter"]["Name"] ?? "";
                                authProvider = "twitter";
                                authProviderUserId = user.UserId;
                                break;

                        }

                        return
                            new UserRegistrationPageNavigationParameter("LoginPage")
                            {
                                Email = email,
                                Username = username,
                                AuthProvider = authProvider,
                                AuthProviderUserId = authProviderUserId
                            };


                    }
                }


                return null;
            }
            catch
                (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }
    }
}
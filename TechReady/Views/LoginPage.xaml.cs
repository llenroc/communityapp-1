using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.ApplicationInsights;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using TechReady.Helpers.MessageHelper;
using TechReady.Helpers.NetworkHelper;
using TechReady.Helpers.Telemetry;
using TechReady.NavigationParameters;
using TechReady.ViewModels;
using TechReady.Common.DTO;
using TechReady.Helpers;
using TechReady.Helpers.ServiceCallers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TechReady.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {

        public LoginPage()
        {
            this.InitializeComponent();

            this.DataContext = new LoginPageViewModel();
        }

        public LoginPageViewModel ViewModel
        {
            get
            {
                if (this.DataContext != null)
                {
                    return this.DataContext as LoginPageViewModel;
                }
                return null;
            }
        }

        private async void facebookLogin_Tap(object sender, TappedRoutedEventArgs e)
        {
            await this.AuthenticateAsync(MobileServiceAuthenticationProvider.Facebook);
        }

        private async void twitterLogin_Tap(object sender, TappedRoutedEventArgs e)
        {
            await this.AuthenticateAsync(MobileServiceAuthenticationProvider.Twitter);
        }

        private async void microsoftLogin_Tap(object sender, TappedRoutedEventArgs e)
        {
            await this.AuthenticateAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);
        }

        // Define a member variable for storing the signed-in user. 
        private MobileServiceUser user;

        // Define a method that performs the authentication process
        // using a Facebook sign-in. 
        private async System.Threading.Tasks.Task AuthenticateAsync(MobileServiceAuthenticationProvider provider)
        {
            if (NetworkHelper.IsNetworkAvailable() == false)
            {
                await MessageHelper.ShowMessage(CommonSettings.LoginNoNetworkMessage);
                return;
            }


            this.ViewModel.OperationInProgress = true;
            string username="",email="",authProvider="",authProviderUserId="";
            try
            {
                // Change 'MobileService' to the name of your MobileServiceClient instance.
                // Sign-in using Facebook authentication.
                user = await App.TechReadyServicesClient
                    .LoginAsync(provider);

                if (user != null)
                {
                    var userInfo = await App.TechReadyServicesClient.InvokeApiAsync("UserInfo", HttpMethod.Get, null);
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
                        
                        Frame.Navigate(typeof (UserRegisterationPage), JsonConvert.SerializeObject(
                            new UserRegistrationPageNavigationParameter("LoginPage")
                            {
                                Email = email,
                                Username = username,
                                AuthProvider = authProvider,
                                AuthProviderUserId = authProviderUserId
                            }));
                    }

                }


            }
            catch(Exception ex)
            {
                TelemetryCollector.ReportExepction(ex);
            }
            finally
            {
                this.ViewModel.OperationInProgress = false;
            }

            //var dialog = new MessageDialog(message);
            //dialog.Commands.Add(new UICommand("OK"));
            //await dialog.ShowAsync();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (AboutUsPage));
        }
    }
}

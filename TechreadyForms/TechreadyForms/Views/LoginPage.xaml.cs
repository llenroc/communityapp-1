using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using TechreadyForms.Helpers.AuthenticationHelper;
using TechReady.Helpers;
using TechReady.Helpers.MessageHelper;
using Xamarin.Forms;
using TechReady.ViewModels;

namespace TechreadyForms.Views
{
    public partial class LoginPage : ContentPage
    {
        ToolbarItem aboutToolbarItem;
        private IAuthenticationHelper authHelper;
        public LoginPage()
        {
            aboutToolbarItem = new ToolbarItem("About", "helpIcon.png", this.About_Click, 0, 0);

            InitializeComponent();
            this.Appearing += LoginPage_Appearing;
            this.BindingContext = new LoginPageViewModel();

            authHelper = DependencyService.Get<IAuthenticationHelper>();

        }

        private void LoginPage_Appearing(object sender, EventArgs e)
        {
            this.ToolbarItems.Clear();
            this.ToolbarItems.Add(aboutToolbarItem);
        }

        public LoginPageViewModel ViewModel
        {
            get
            {
                if (this.BindingContext != null)
                {
                    return this.BindingContext as LoginPageViewModel;
                }
                return null;
            }
        }

        private async void Facebook_Tapped(object sender, EventArgs e)
        {
            this.Authenticate(MobileServiceAuthenticationProvider.Facebook);
        }

        private async void Twitter_Tapped(object sender, EventArgs e)
        {
            this.Authenticate(MobileServiceAuthenticationProvider.Twitter);
        }

        private async void Microsoft_Tapped(object sender, EventArgs e)
        {
          this.Authenticate(MobileServiceAuthenticationProvider.MicrosoftAccount);
        }

        private void About_Click()
        {
            Navigation.PushAsync(new AboutUsPage());
        }


        private async void Authenticate(MobileServiceAuthenticationProvider provider)
        {

            if (TechReady.Helpers.NetworkHelper.NetworkHelper.IsNetworkAvailable() == false)
            {
                await MessageHelper.ShowMessage(CommonSettings.LoginNoNetworkMessage);
                return;
            }

            try
            {
                this.ViewModel.OperationInProgress = true;

                var userInfo = await authHelper.Authenticate(provider);
                if (userInfo != null)
                {
                    var userPageViewModel = new UserRegistrationPageViewModel();

                    userPageViewModel.FullName = userInfo.Username;
                    userPageViewModel.Email = userInfo.Email;
                    userPageViewModel.AuthProvider = userInfo.AuthProvider;
                    userPageViewModel.AuthProviderUserId = userInfo.AuthProviderUserId;
                    if (await userPageViewModel.GetTechnologes())
                    {
                        Navigation.PushAsync(new HubPage(null));

                        //Clear Backstack
                        for (int i = 0; i < this.Navigation.NavigationStack.Count - 1; i++)
                        {
                            this.Navigation.RemovePage(this.Navigation.NavigationStack[i]);
                        }
                    }
                    else
                    {
                        Navigation.PushAsync(new UserRegistrationPage(userPageViewModel));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

            }
            finally
            {
                this.ViewModel.OperationInProgress = false;
            }

        }

    }
}

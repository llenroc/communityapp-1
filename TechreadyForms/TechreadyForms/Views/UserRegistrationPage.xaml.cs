using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TechReady.Helpers.MessageHelper;
using TechReady.Helpers.NetworkHelper;
using TechReady.Helpers.Storage;
using TechReady.NavigationParameters;
using TechReady.ViewModels;
using Xamarin.Forms;
using TechreadyForms.Helpers.DisplayAlertHelper;

namespace TechreadyForms.Views
{
    public partial class UserRegistrationPage : ContentPage
    {
        public UserRegistrationPage(object parameter)
        {
            InitializeComponent();

            if (parameter is UserRegistrationPageViewModel)
            {
                this.BindingContext = parameter as UserRegistrationPageViewModel;
            }
            else
            {
                this.BindingContext = new UserRegistrationPageViewModel();
            }
            this.NavigatedTo(parameter);

        }

        private async void NavigatedTo(object parameter)
        {
            if (parameter.ToString() == "profile")
            {
                this.Title = "Profile";
                //await ((UserRegistrationPageViewModel)this.DataContext).GetTechnologes();

               // appBarButton.Text = "Update";

                var context = await LocalStorage.ReadJsonFromFile<UserRegistrationPageViewModel>("registration");
                if (context != null)
                {
                    this.BindingContext = context;
                    context.OperationInProgress = false;
                }
            }
        }

        public UserRegistrationPageViewModel ViewModel
        {
            get
            {
             
                return this.BindingContext as UserRegistrationPageViewModel;
            }
        }


        private async void Accept_click(object sender, EventArgs e)
        {
            this.ViewModel.OperationInProgress = true;

            try
            {
                if (NetworkHelper.IsNetworkAvailable() == false)
                {
                    await MessageHelper.ShowMessage("Please connect to internet to register/update your profile");
                    return;
                }
                if (await this.ViewModel.SaveUserRegitration())
                {
                    if (this.Title == "Profile")
                    {
                        Navigation.PopAsync(true);
                    }
                    else
                    {

                        await Navigation.PushAsync(new HubPage(null));
                        var existingPages = Navigation.NavigationStack.ToList();
                        for (int i = 0; i < existingPages.Count - 1; i++)
                        {
                            Navigation.RemovePage(existingPages[i]);
                        }
                    }
                }
                else
                {

                    if (this.ViewModel.IsValid)
                        await MessageHelper.ShowMessage("Unable to connect with server. Please check your network connection and try again.");
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

        private async void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            //MessageHelper.ShowMessage(
            //   "Email address is required to create a personalized app experience based on your role & technology preferences. We may also use the information to communicate with you in case of any app feedback you might share with us.");

            var pressedPrivacy = await DisplayAlertHelper.DisplayMessage("Alert", "Email address is required to create a personalized app experience based on your role & technology preferences. We may also use the information to communicate with you in case of any app feedback you might share with us.","OK", "Privacy Statement");
        
            if (pressedPrivacy)
            {
                var ActionLink = "http://www.microsoft.com/privacystatement/en-in/MicrosoftIndiaWebsites/Default.aspx";

                if(Device.OS == TargetPlatform.iOS)
                {
                    Uri actionLink;
                    if(Uri.TryCreate(ActionLink, UriKind.Absolute, out actionLink))
                    {
                        Device.OpenUri(actionLink);
                    }
                }

                else
                {
                    await Navigation.PushAsync(new WebViewPage(ActionLink));
                }
            }

        }

    }
}
 
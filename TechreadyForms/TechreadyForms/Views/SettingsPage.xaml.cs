using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechReady.Helpers.Storage;
using TechReady.ViewModels;
using Xamarin.Forms;

namespace TechreadyForms.Views
{
    public partial class SettingsPage : ContentPage
    {
        private object _parameter;
        //private ToolbarItem logoutToolbarItem;

        public SettingsPage()
        {
            this.Appearing += SettingsPage_Appearing;
            //logoutToolbarItem = new ToolbarItem("Logout", "logoutIcon.png", this.logout_click, 0, 0);
            InitializeComponent();
            this.BindingContext = new SettingsPageViewModel();
        }

        private void SettingsPage_Appearing(object sender, EventArgs e)
        {
            this.NavigatedTo();
        }

        private void NavigatedTo()
        {
            //this.ToolbarItems.Clear();
            //this.ToolbarItems.Add(logoutToolbarItem);
            ((SettingsPageViewModel)this.BindingContext).LoadSettings();
        }

        //private void NavigatedTo(object parameter)    
        //{
        //    ((SettingsPageViewModel)this.BindingContext).LoadSettings();
        //}
        private async void logout_tapped(object sender, EventArgs e)
        {
            await LocalStorage.DeleteJsonFromFile("registration");
            //await LocalStorage.RemoveAllFiles();
            App.Current.MainPage = new NavigationPage(new LoginPage()) { };
            //App.Current.MainPage = new LoginPage();
        }
        void Notifications_Toggeled(object sender, ToggledEventArgs e)
        {
            ((SettingsPageViewModel)this.BindingContext).SaveSettings();
        }

        private void profile_tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UserRegistrationPage("profile"));
        }

        private void about_tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AboutUsPage());
        }
    }
}

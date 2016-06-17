using TechReady.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TechReady.Helpers.Storage;
using TechReady.ViewModels;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TechReady.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public SettingsPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.DataContext = new SettingsPageViewModel();
            this.ViewModel.LoadSettings();
        }

        public SettingsPageViewModel ViewModel
        {
            get { return this.DataContext as SettingsPageViewModel; }
        }


        private void profile_tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof (UserRegisterationPage), "profile");
        }

        private async void Notifications_Toggeled(object sender, RoutedEventArgs e)
        {
            this.ViewModel.SaveSettings();

        }

        private void about_tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(AboutUsPage));
        }



        private async void logout_click(object sender, RoutedEventArgs e)
        {
            await LocalStorage.DeleteJsonFromFile("registration");
            await LocalStorage.RemoveAllFiles();
            Frame.Navigate(typeof(LoginPage));
            Frame.BackStack.Clear();
            //Application.Current.Exit();
        }
    }
}

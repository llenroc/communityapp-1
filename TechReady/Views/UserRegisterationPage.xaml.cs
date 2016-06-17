using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using TechReady.Helpers.Storage;
using TechReady.NavigationParameters;
using TechReady.ViewModels;
using TechReady.Helpers.NetworkHelper;
using TechReady.Helpers.MessageHelper;
using TechReady.UserControls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TechReady.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserRegisterationPage : Page
    {
        public UserRegisterationPage()
        {
            this.InitializeComponent();
            this.DataContext = new UserRegistrationPageViewModel();

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter != null && e.Parameter.ToString()!="profile")
            {
                var np = JsonConvert.DeserializeObject<UserRegistrationPageNavigationParameter>(e.Parameter.ToString());

                if (np != null)
                {
                    if (np.FromPageName == "LoginPage")
                    {
                        ((UserRegistrationPageViewModel) this.DataContext).FullName = np.Username;
                        ((UserRegistrationPageViewModel) this.DataContext).Email = np.Email;
                        ((UserRegistrationPageViewModel) this.DataContext).AuthProvider = np.AuthProvider;
                        ((UserRegistrationPageViewModel) this.DataContext).AuthProviderUserId = np.AuthProviderUserId;
                        if (await ((UserRegistrationPageViewModel)this.DataContext).GetTechnologes())
                        {
                            Frame.Navigate(typeof(HubPage));
                            Frame.BackStack.Clear();
                        }
                        else
                        {
                            if(!( await ((UserRegistrationPageViewModel)this.DataContext).CheckDataAndShowMessage()))
                            {
                                if (Frame.CanGoBack)
                                {
                                    Frame.GoBack();
                                }
                            }
                        }
                    }
                }
                
            }
            if (e.Parameter.ToString() == "profile")
            {
                this.HeadingText.Text = "profile";
                //await ((UserRegistrationPageViewModel)this.DataContext).GetTechnologes();

                appBarButton.Label = "Update";

                var context = await LocalStorage.ReadJsonFromFile<UserRegistrationPageViewModel>("registration");
                if (context != null)
                {
                    this.DataContext = context;
                    context.OperationInProgress = false;
                }
            }
            else
            {
               
            }
        }


    
      

        private async void Accept_click(object sender, RoutedEventArgs e)
        {

            if (NetworkHelper.IsNetworkAvailable() == false)
            {
                await MessageHelper.ShowMessage("Please connect to internet to register/update your profile");
                return;
            }
            if (await ((UserRegistrationPageViewModel)this.DataContext).SaveUserRegitration())
            {
                if (this.HeadingText.Text == "profile")
                {
                    Frame.GoBack();
                }
                else
                {
                    Frame.Navigate(typeof(HubPage));
                    Frame.BackStack.Clear();
                }
            }
            else
            {
                if (((UserRegistrationPageViewModel)this.DataContext).IsValid)
                    await MessageHelper.ShowMessage("Unable to connect with server. Please check your network connection and try again.");
            }

        }

        private void EmailInfo_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Popup p = new Popup();
            var c = new EmailInfoMessage();
          
            c.OKTapped += () =>
            {
                p.IsOpen = false;
            };

            c.PrivacyStatementTapped += () =>
            {
                Frame.Navigate(typeof (EventRegistrationPage),
                    @"http://www.microsoft.com/privacystatement/en-in/MicrosoftIndiaWebsites/Default.aspx");
                p.IsOpen = false;
            };

            p.Child = c;

            CoreWindow yourContainingWindow = Window.Current.CoreWindow;
            Rect windowBounds = yourContainingWindow.Bounds;
            Point windowCenter = new Point(windowBounds.Left + (windowBounds.Width / 2.0), windowBounds.Top + (windowBounds.Height / 2.0));
            p.SetValue(Popup.VerticalOffsetProperty, (windowCenter.Y-p.ActualHeight) / 2.0);
            //p.SetValue(Popup.HorizontalOffsetProperty,(this.ActualWidth)/2.0);

            p.IsOpen = true;

           
        }
    }
}

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
    public sealed partial class FeedbackPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public FeedbackPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.DataContext = new FeedbackPageViewModel();
        }

   
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            var context = await LocalStorage.ReadJsonFromFile<UserRegistrationPageViewModel>("registration");
            ((FeedbackPageViewModel)DataContext).Email = context.Email;
            ((FeedbackPageViewModel)DataContext).UserName = context.FullName;
          

        }

 

        private async void SubmitFeedback_Click(object sender, RoutedEventArgs e)
        {
            if (await ((FeedbackPageViewModel)DataContext).SaveFeedback())
            {
            }
        }
        
        private void ResponseMessage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}

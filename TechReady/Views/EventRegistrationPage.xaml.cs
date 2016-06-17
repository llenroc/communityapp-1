using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TechReady.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TechReady.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EventRegistrationPage : Page
    {
        public EventRegistrationPage()
        {
            this.InitializeComponent();

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null)
            {
                try
                {
                    var uri = new Uri(e.Parameter.ToString());
                    this.WebView.Navigate(uri);
                }
                catch
                {
                    this.ProgressBar.Visibility = Visibility.Collapsed;
                    this.FailureGrid.Visibility = Visibility.Visible;
                    FailureGrid_Tapped(null, null);
                }
                
            }
           
        }


        private void WebView_OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            this.ProgressBar.Visibility = Visibility.Visible;
        }

        private void WebView_OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            this.ProgressBar.Visibility = Visibility.Collapsed;
        }

        private void WebView_OnNavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            this.ProgressBar.Visibility = Visibility.Collapsed;
            this.FailureGrid.Visibility = Visibility.Visible;
        }

        private void FailureGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}

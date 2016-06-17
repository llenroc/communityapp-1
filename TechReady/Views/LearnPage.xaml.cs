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
using Newtonsoft.Json;
using TechReady.Models;
using TechReady.Helpers.Storage;
using Windows.ApplicationModel.DataTransfer;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TechReady.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LearnPage : Page
    {
        private static LearningResource _resource = null;
        public LearnPage()
        {
            this.InitializeComponent();
            this.DataContext = new LearnPageViewModel();
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
                _resource = JsonConvert.DeserializeObject<LearningResource>(e.Parameter as string);

                this.HeadingText.Text = _resource.Title;
                var uri = new Uri(_resource.Link);
                this.WebView.Navigate(uri);

                //((LearnPageViewModel)this.DataContext).Watched_Videos(_resource);

                DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
                dataTransferManager.DataRequested +=
                    new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.DataRequested);
            }
        }

        private void DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var resourceData = _resource as LearningResource;

            DataRequest request = args.Request;
            DataRequestDeferral deferral = request.GetDeferral();
            // Make sure we always call Complete on the deferral.
            try
            {
                request.Data.Properties.Title = "Useful resource";
                request.Data.Properties.ApplicationName = "Tech Ready";

                if (!string.IsNullOrEmpty(resourceData.Link))
                {
                    request.Data.SetText(string.Format("{0}, Link {1}", resourceData.Title, resourceData.Link));
                }
            }
            finally
            {
                deferral.Complete();
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
        }
        private void Share_Clicked(object sender, RoutedEventArgs e)
        {
            Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
        }

    }
}

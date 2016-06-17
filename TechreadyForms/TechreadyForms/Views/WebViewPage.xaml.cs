using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace TechreadyForms.Views
{
    public partial class WebViewPage : ContentPage
    {
        private object _parameter;

        public WebViewPage(object parameter)
        {
            InitializeComponent();
            _parameter = parameter;

            this.WebView.Navigating += this.WebView_OnNavigating;
            this.WebView.Navigated += this.WebView_OnNavigated;


        }

        private void WebView_OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            this.ProgressBar.IsRunning = true;
            this.ProgressBar.IsVisible = true;
        }

        private void WebView_OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            this.ProgressBar.IsVisible = false;
            this.ProgressBar.IsRunning = false;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected void NavigatedTo()
        {
            if (_parameter != null)
            {
                try
                {
                    var uri = new Uri(_parameter.ToString());
                    this.WebView.Source = new UrlWebViewSource()
                    {
                        Url = _parameter.ToString()
                    };
                    this.ProgressBar.IsRunning = true;
                    this.ProgressBar.IsVisible = true;
                }
                catch
                {
                    this.ProgressBar.IsVisible = false;
                    this.ProgressBar.IsRunning = false;
                    this.ErrorMessage.IsVisible = true;
                }

            }

        }

        private void WebViewPage_OnAppearing(object sender, EventArgs e)
        {
            this.NavigatedTo();
        }
    }
}

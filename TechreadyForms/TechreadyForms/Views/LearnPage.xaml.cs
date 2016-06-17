using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechreadyForms.Helpers;
using TechreadyForms.Helpers.ShareHelper;
using System.Threading.Tasks;

using Xamarin.Forms;
using Newtonsoft.Json;
using TechReady.Models;

namespace TechreadyForms.Views
{
    public partial class LearnPage : ContentPage
    {
        private ToolbarItem shareToolbarItem;
        private LearningResource _resource = null;
        private LearningResource _parameter;
        public LearnPage(LearningResource parameter)
        {
            this._parameter = parameter;
            this.Appearing += LearnPage_Appearing;
            shareToolbarItem = new ToolbarItem("Share", "Share.png", this.ShareEvent);

            this.ToolbarItems.Add(shareToolbarItem);

            InitializeComponent();

            this.BindingContext = new TechReady.ViewModels.LearnPageViewModel();
          
        }

        private void LearnPage_Appearing(object sender, EventArgs e)
        {
            this.NavigatedTo();

            this.WebView.Navigating += this.WebView_OnNavigationStarting;
            this.WebView.Navigated += this.WebView_OnNavigationCompleted;

        }

        public void ShareEvent()
        {

            try
            {

                this.ProgressBar.IsRunning = true;
                this.ProgressBar.IsVisible = true;
                var resourceData = _resource as LearningResource;

                StringBuilder builder = new StringBuilder();
                builder.Append("Useful resource");
                builder.Append("\n");
                builder.Append(string.Format("{0}, Link {1}", resourceData.Title, resourceData.Link));

                ShareHelper.Share(builder.ToString());
            }
            catch (Exception ex)
            {

            }
            finally
            {
                this.ProgressBar.IsRunning = false;
                this.ProgressBar.IsVisible = false;
            }
        }

        private void NavigatedTo()
        {
            if (_parameter != null)
            {

                try
                {
                    _resource = _parameter;
                   
                    var uri = new Uri(_resource.Link);

                    this.WebView.Source = new UrlWebViewSource()
                    {
                        Url = _resource.Link.ToString()
                    };

                    ((TechReady.ViewModels.LearnPageViewModel)this.BindingContext).Watched_Videos(_resource);
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

        void WebView_OnNavigationStarting(object sender, WebNavigatingEventArgs e)
        {
            this.ProgressBar.IsRunning = true;
            this.ProgressBar.IsVisible = true;
        }

        void WebView_OnNavigationCompleted(object sender, WebNavigatedEventArgs e)
        {
            this.ProgressBar.IsRunning = false;
            this.ProgressBar.IsVisible = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TechReady.Models;
using TechReady.ViewModels;
using Xamarin.Forms;

namespace TechreadyForms.Views
{
    public partial class WatchedVideosListPage : ContentPage
    {
        public WatchedVideosListPage()
        {
            this.BindingContext = new WatchedVideosListPageViewModel();
            InitializeComponent();
        }

        public WatchedVideosListPageViewModel ViewModel { get { return this.BindingContext as WatchedVideosListPageViewModel;} }

        private async void WatchedVideosListPage_OnAppearing(object sender, EventArgs e)
        {
            await(this.ViewModel).GetWatchedVideos();
        }

        private void LearningResource_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event

            if (e.Item is LearningResource)
            {

                try
                {
                    Uri uri = new Uri(((LearningResource)e.Item).Link);

                    //Navigation.PushAsync(new LearnPage((LearningResource)e.Item));    // to open page within in device
                    Device.OpenUri(uri);
                    (new LearnPageViewModel()).Watched_Videos(e.Item);
                }
                catch
                {
                }
            }

            Debug.WriteLine("Tapped: " + e.Item);
            ((ListView) sender).SelectedItem = null; // de-select the row
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechReady.Common.Models;
using TechReady.ViewModels;
using Xamarin.Forms;

namespace TechreadyForms.Views
{
    public partial class FollowedSpeakersPage : ContentPage
    {
        public FollowedSpeakersPage()
        {
            this.BindingContext = new FollowedSpeakersViewModel();

            InitializeComponent();
        }

        public FollowedSpeakersViewModel ViewModel
        {
            get
            {
                return this.BindingContext as FollowedSpeakersViewModel;

            }
        }

        private async void SpeakersListPage_OnAppearing(object sender, EventArgs e)
        {
            await this.ViewModel.GetFollowedSpeakers();

        }

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {

            if (e.Item != null && e.Item is TrackSpeaker)
            {
                Navigation.PushAsync(new SpeakerDetailsPage((TrackSpeaker) e.Item));
            }

            this.SpeakersList.SelectedItem = null;
        }
    }
}
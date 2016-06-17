using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TechReady.Common.Models;
using TechReady.ViewModels;
using Xamarin.Forms;

namespace TechreadyForms.Views
{
    public partial class SpeakersListPage : ContentPage
    {

        private ToolbarItem searchToolbarItem;

        private ToolbarItem resetToolbarItem;

        public SpeakersListPage()
        {
            this.BindingContext = new SpeakersListPageViewModel();

            searchToolbarItem = new ToolbarItem("Search", "SearchIcon.png", this.SearchButton_Click, 0, 0);
            resetToolbarItem = new ToolbarItem("Reset", "resetIcon.png", this.ResetButton_Click, 0, 0);
            InitializeComponent();
        }

        public SpeakersListPageViewModel ViewModel
        {
            get
            {
                return this.BindingContext as SpeakersListPageViewModel;

            }
        }

        private async void SpeakersListPage_OnAppearing(object sender, EventArgs e)
        {

            if (this.ViewModel.Speakers == null)
            {
                await this.ViewModel.GetSpeakers();
                this.ResetButton_Click();
            }
        }

        private void SearchSpeaker_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.ViewModel != null)
            {
                this.ViewModel.Search_Speakers(e.NewTextValue);
            }
        }

        private void SearchButton_Click()
        {
            if (this.ViewModel != null)
                this.ViewModel.SearchOpen = true;
            this.ToolbarItems.Remove(searchToolbarItem);
            this.ToolbarItems.Add(resetToolbarItem);
        }

        private void ResetButton_Click()
        {
            this.ToolbarItems.Clear();
            this.ToolbarItems.Add(searchToolbarItem);
            if (this.ViewModel != null)
            {
                this.ViewModel.SearchOpen = false;
                if (this.ViewModel.AreSpeakersAvailable)
                    this.SearchBox.Text = "";
            }
        }

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null && e.Item is TrackSpeaker)
            {
                Navigation.PushAsync(new SpeakerDetailsPage((TrackSpeaker)e.Item));
            }

            this.SpeakersList.SelectedItem = null;
        }
    }
}

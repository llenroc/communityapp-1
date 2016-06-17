using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TechReady.Common.Models;
using TechReady.ViewModels;
using Xamarin.Forms;

namespace TechreadyForms.Views
{
    public partial class FollowedEventsPage : ContentPage
    {
        private object _parameter;

        public FollowedEventsPage(object parameter)
        {
            this.BindingContext = new FollowedEventsViewModel();


            InitializeComponent();

            _parameter = parameter;
        }

        private FollowedEventsViewModel ViewModel
        {
            get { return this.BindingContext as FollowedEventsViewModel; }
        }

        private async void NavigatedTo(object parameter)
        {

            this.ErrorMessage.IsVisible = false;
            await this.ViewModel.GetFollowedEvents();
            if (this.ViewModel.AllEvents == null || (this.ViewModel.AllEvents.Count == 0))
            {
                this.ErrorMessage.IsVisible = true;
                this.ErrorMessage.Text = "Looks like you have not followed any event";
            }
        }


        private void Event_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event

            Navigation.PushAsync(new EventDetails((Event)e.Item));
            Debug.WriteLine("Tapped: " + e.Item);
            ((ListView) sender).SelectedItem = null; // de-select the row
        }

        private void FollowedEventsPage_OnAppearing(object sender, EventArgs e)
        {
            this.NavigatedTo(_parameter);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TechreadyForms.Helpers;
using TechReady.Common.Models;
using TechReady.Helpers.Storage;
using TechReady.Models;
using TechReady.NavigationParameters;
using TechReady.ViewModels;
using Xamarin.Forms;

namespace TechreadyForms.Views
{
    public partial class HubPage : TabbedPage
    {
        private ToolbarItem filterToolbarItem;

        private ToolbarItem resetToolbarItem;


        public HubPage(object parameter)
        {
            this.BindingContext = new HubPageViewModel();

            filterToolbarItem = new ToolbarItem("Filter", "filterIcon.png", this.Filter_Tapped, 0, 0);
            resetToolbarItem = new ToolbarItem("Reset", "resetIcon.png", this.ResetFilter_Tapped, 0, 0);

            InitializeComponent();
            this.NavigatedTo(parameter);
        }

        private HubPageViewModel ViewModel
        {
            get { return this.BindingContext as HubPageViewModel; }
        }

        private async void NavigatedTo(object parameter)
        {

            if (parameter != null)
            {
                var np = JsonConvert.DeserializeObject<NavigationParameter>(parameter.ToString());
                if (np != null)
                {
                    if (np.FromPageName == "FilterEventsPage")
                    {
                        this.SelectedItem = this.Children[0];
                        var fnp =
                            JsonConvert.DeserializeObject<EventsFilterPageNavigationParameters>(parameter.ToString());
                        if (fnp != null)
                        {
                            this.ViewModel.CurrentFilterCriteria = fnp;
                            this.Attend_OnAppearing(null, null);
                            //await
                            //   this.ViewModel.FilterEvents(fnp.Location, fnp.Technology,
                            //        fnp.Role);
                        }
                        //ToolbarItems.Add(filterToolbarItem);
                        //ToolbarItems.Add(resetToolbarItem);

                    }
                    if (np.FromPageName == "FilterLearningResourcesPage")
                    {

                        this.SelectedItem = this.Children[1];
                        var fnp =
                            JsonConvert.DeserializeObject<LearningResourcesFilterPageNavigationParameters>(
                                parameter.ToString());
                        if (fnp != null)
                        {
                            this.ViewModel.CurrentLearningResourcesFilterCriteria = fnp;
                            this.Learn_OnAppearing(null, null);
                            //await
                            //    this.ViewModel.FilterLearningResources(fnp.Type, fnp.Technology,
                            //        fnp.Role);
                        }
                        //ToolbarItems.Add(filterToolbarItem);
                        //ToolbarItems.Add(resetToolbarItem);


                    }
                }
            }

            if (this.SelectedItem != null && ((ContentPage) this.SelectedItem).Title == "Inbox")
            {
                await this.ViewModel.GetNotifications();
            }

            var data = await LocalStorage.ReadJsonFromFile<List<TrackSpeaker>>("followedSpeakers");
            if (data == null || data.Count == 0)
            {
                this.ViewModel.SpeakersTileList = false;
                return;
            }


            this.ViewModel.SpeakersTileList = true;
            this.ViewModel.AddOnlyLimitedSpeakers(data, 8);

        }

        private async void Attend_OnAppearing(object sender, EventArgs e)
        {
            if (this.ViewModel.CurrentFilterCriteria == null)
            {
                await this.ViewModel.GetEvents();
            }
            else
            {
                await
                    this.ViewModel.FilterEvents(this.ViewModel.CurrentFilterCriteria.Location,
                        this.ViewModel.CurrentFilterCriteria.Technology,
                        this.ViewModel.CurrentFilterCriteria.Role);

                if (!ToolbarItems.Contains(filterToolbarItem))
                    ToolbarItems.Add(filterToolbarItem);
                if (!ToolbarItems.Contains(resetToolbarItem))
                    ToolbarItems.Add(resetToolbarItem);
            }

        }

        private async void Learn_OnAppearing(object sender, EventArgs e)
        {
            if (this.ViewModel.CurrentLearningResourcesFilterCriteria == null)
            {
                await this.ViewModel.GetLearningContent();
            }
            else
            {

                await
                    this.ViewModel.FilterLearningResources(this.ViewModel.CurrentLearningResourcesFilterCriteria.Type,
                        this.ViewModel.CurrentLearningResourcesFilterCriteria.Technology,
                        this.ViewModel.CurrentLearningResourcesFilterCriteria.Role);
                if (!ToolbarItems.Contains(filterToolbarItem))
                    ToolbarItems.Add(filterToolbarItem);
                if (!ToolbarItems.Contains(resetToolbarItem))
                    ToolbarItems.Add(resetToolbarItem);
            }
        }

        private async void Inbox_OnAppearing(object sender, EventArgs e)
        {
            await this.ViewModel.GetNotifications();
        }

        private async void MyZone_OnAppearing(object sender, EventArgs e)
        {
            var data = await LocalStorage.ReadJsonFromFile<List<TrackSpeaker>>("followedSpeakers");
            if (data == null || data.Count == 0)
            {
                this.ViewModel.SpeakersTileList = false;
                return;
            }
            this.ViewModel.SpeakersTileList = true;
            this.ViewModel.AddOnlyLimitedSpeakers(data, 8);
            //this.SpeakersTile.ItemsSource = data;
        }

        private void HubPage_OnCurrentPageChanged(object sender, EventArgs e)
        {
            ToolbarItems.Clear();
            this.ViewModel.ShowAll = false;
        }

        private void Event_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event

            if (e.Item is LoadMore)
            {
                this.ViewModel.ShowAll = true;

                if (!ToolbarItems.Contains(filterToolbarItem))
                    ToolbarItems.Add(filterToolbarItem);
                if (!ToolbarItems.Contains(resetToolbarItem))
                    ToolbarItems.Add(resetToolbarItem);

            }
            else if (e.Item is Event)
            {
                Navigation.PushAsync(new EventDetails(((Event) e.Item)));
            }
            Debug.WriteLine("Tapped: " + e.Item);
            ((ListView) sender).SelectedItem = null; // de-select the row
        }


        private void LearningResource_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event

            if (e.Item is LoadMoreLearningResource)
            {
                this.ViewModel.ShowAll = true;

                if (!ToolbarItems.Contains(filterToolbarItem))
                    ToolbarItems.Add(filterToolbarItem);
                if (!ToolbarItems.Contains(resetToolbarItem))
                    ToolbarItems.Add(resetToolbarItem);

            }
            else if (e.Item is LearningResource)
            {
                try
                {
                    Uri uri = new Uri(((LearningResource) e.Item).Link);

                    //Navigation.PushAsync(new LearnPage((LearningResource)e.Item));    // to open page within in device
                    Device.OpenUri(uri);
                    //(new LearnPageViewModel()).Watched_Videos(e.Item);
                }
                catch
                {

                }
            }
            Debug.WriteLine("Tapped: " + e.Item);
            ((ListView) sender).SelectedItem = null; // de-select the row
        }

        private void Filter_Tapped()
        {
            if (((ContentPage) CurrentPage).Title == "Attend")
            {
                if (this.ViewModel.CurrentFilterCriteria == null)
                {
                    Navigation.PushAsync(new FilterEventsPage(null));
                }
                else
                {
                    Navigation.PushAsync(new FilterEventsPage(
                        this.ViewModel.CurrentFilterCriteria));
                }
            }
            else if (((ContentPage) CurrentPage).Title == "Learn")
            {
                if (this.ViewModel.CurrentLearningResourcesFilterCriteria == null)
                {
                    Navigation.PushAsync(new LearningResourcesFilteredPage(null));
                    // Frame.Navigate(typeof(FilterLearningResourcesPage));
                }
                else
                {
                    Navigation.PushAsync(new LearningResourcesFilteredPage(
                        this.ViewModel.CurrentLearningResourcesFilterCriteria));

                }
            }
        }

        private async void Notification_Tapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView) sender).SelectedItem = null; // de-select the row

            if (e == null)
            {
                return;
            }
            var action = await DisplayActionSheet("", "Cancel", null, "Read", "Remove");
            switch (action)
            {
                case "Read":
                    if (e.Item is Notification)
                    {
                        var notification = e.Item as Notification;
                        ((HubPageViewModel) this.BindingContext).Notification_Tapped(notification);

                        if (!string.IsNullOrEmpty(notification.ActionLink))
                        {
                            Uri actionLink;
                            if (Uri.TryCreate(notification.ActionLink, UriKind.Absolute, out actionLink))
                            {
                                Navigation.PushAsync(new WebViewPage(notification.ActionLink));
                            }
                        }
                    }
                    break;

                case "Remove":
                    if (e.Item is Notification)
                    {
                        var notification = e.Item as Notification;
                        ((HubPageViewModel) this.BindingContext).Notification_Tapped(notification);

                        this.ViewModel.RemoveNotification_Click(notification);
                    }
                    break;

            }
            Debug.WriteLine("Action: " + action); // writes the selected button label to the console

            Debug.WriteLine("Tapped: " + e.Item);
        }

        private async void ResetFilter_Tapped()
        {
            if (((ContentPage) CurrentPage).Title == "Attend")
            {
                //TODO: test this feature after placing this loc in view model, for windows device
                this.ViewModel.CurrentFilterCriteria = null;
                    // this line of code must be shifted to view model in function ResetFiltersEvent()
                await this.ViewModel.ResetFiltersEvent();
            }
            if (((ContentPage) CurrentPage).Title == "Learn")
            {
                //TODO: test this feature after placing this loc in view model, for windows device
                this.ViewModel.CurrentLearningResourcesFilterCriteria = null;
                    // this line of code must be shifted to view model in function ResetFiltersLearningResources()
                await this.ViewModel.ResetFiltersLearningResources();
            }

            ToolbarItems.Clear();
            //ToolbarItems.Remove(filterToolbarItem);
            //ToolbarItems.Remove(resetToolbarItem);

        }


        private void FollowedEvents_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FollowedEventsPage(null));
        }

        private void Speakeers_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SpeakersListPage());
        }

        private void FollowedSpeakeers_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FollowedSpeakersPage());
        }

        private void WatchedVideos_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new WatchedVideosListPage());
        }

        private void Feedback_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FeedbackPage());
        }

        private void Settings_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPage());
        }


        protected override bool OnBackButtonPressed()
        {

            TechReady.Helpers.MessageHelper.MessageHelper.ShowMessage(
                "Do you want to exit from application?", "", "Ok", "Cancel",
                () =>
                {
                    var service = DependencyService.Get<IAppLifeHelper>();
                    service.Kill();
                });
            return true;






        }

    }
}

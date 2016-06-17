using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Email;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using TechReady.Common.Models;
using TechReady.Models;
using TechReady.NavigationParameters;
using TechReady.ViewModels;
using TechReady.Helpers.Storage;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using TechReady.Helpers.MessageHelper;
using TechReady.Helpers.NetworkHelper;
using System.Collections.ObjectModel;
using TechReady.Helpers.ServiceCallers;
using Windows.Graphics.Display;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TechReady.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HubPage : Page
    {
        public HubPage()
        {
            this.InitializeComponent();
            this.DataContext = new HubPageViewModel();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;  //restrict application to rotate

            //Clear Backstack
            if (Frame != null && Frame.BackStack != null && Frame.BackStack.Count > 0)
            {
                foreach (var frame in Frame.BackStack)
                    Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
            }

            if (e.Parameter != null && e.NavigationMode == NavigationMode.New)
            {
                var np = JsonConvert.DeserializeObject<NavigationParameter>(e.Parameter.ToString());
                if (np != null)
                {
                    if (np.FromPageName == "FilterEventsPage")
                    {
                        var fnp = JsonConvert.DeserializeObject<EventsFilterPageNavigationParameters>(e.Parameter.ToString());
                        if (fnp != null)
                        {
                            ((HubPageViewModel)this.DataContext).CurrentFilterCriteria = fnp;
                            await
                                ((HubPageViewModel)this.DataContext).FilterEvents(fnp.Location, fnp.Technology,
                                    fnp.Role);
                        }

                    }
                    if (np.FromPageName == "FilterLearningResourcesPage")
                    {
                        var fnp = JsonConvert.DeserializeObject<LearningResourcesFilterPageNavigationParameters>(e.Parameter.ToString());
                        if (fnp != null)
                        {
                            this.ViewModel.CurrentLearningResourcesFilterCriteria = fnp;
                            await
                                ((HubPageViewModel)this.DataContext).FilterLearningResources(fnp.Type, fnp.Technology,
                                    fnp.Role);
                        }

                    }
                }
            }

            if (HubPivot.SelectedItem != null && HubPivot.SelectedIndex == 2)
            {
                await ((HubPageViewModel)this.DataContext).GetNotifications();
            }

            var data = await LocalStorage.ReadJsonFromFile<List<TrackSpeaker>>("followedSpeakers");
            if (data == null || data.Count == 0)
            {
                ((HubPageViewModel)this.DataContext).SpeakersTileList = false;
                return;
            }

            //if (data.Count == 0)
            //{
            //    ((HubPageViewModel)this.DataContext).SpeakersTileList = false;
            //    return;
            //}

                ((HubPageViewModel)this.DataContext).SpeakersTileList = true;
            ((HubPageViewModel)this.DataContext).AddOnlyLimitedSpeakers(data,8);
            //this.SpeakersTile.ItemsSource = data;

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;
            base.OnNavigatedFrom(e);
        }

        private async void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;

            if (((HubPageViewModel)this.DataContext).ShowAll)
            {
                this.Reset_Filter_Clicked(sender, null);
                return;
            }

            if (Frame.BackStack.Count > 0)
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
            }
            else
            {
                await MessageHelper.ShowMessage("Are you sure you wish to exit?", "Exit", "Ok", "Cancel", this.ContentDialog_PrimaryButtonClick);
            }
        }
        
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Application.Current.Exit();
        }

        private void Event_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var grid = sender as Grid;
            if (grid == null)
            {
                return;
            }
            var ev =
            grid.Tag as Event;

            if (ev == null)
            {
                return;
            }

            Frame.Navigate(typeof (EventDetailsPage), JsonConvert.SerializeObject(ev));

        }

        private void Video_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var grid = sender as FrameworkElement;
            if (grid == null)
            {
                return;
            }
            var resource = grid.Tag as LearningResource;
            if (resource == null)
            {
                return;
            }

            //Frame.Navigate(typeof (LearnPage), uri.Link);
            Frame.Navigate(typeof (LearnPage), JsonConvert.SerializeObject(resource));
        }

        private void settings_tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof (SettingsPage));
        }

        private void FollowedEvents_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof (FollowedEvents));
        }

        private async void Feedback_tapped(object sender, TappedRoutedEventArgs e)
        {
            /*
            EmailRecipient sendTo = new EmailRecipient()
            {
                Name = "Tech Ready Development Team",
                Address = "techready-dev@imentor.co.in"
            };

            // Create email object

            EmailMessage mail = new EmailMessage();

            mail.Subject = "Feedback for Teach Ready";


            // Add recipients to the mail object

            mail.To.Add(sendTo);

            //mail.Bcc.Add(sendTo);

            //mail.CC.Add(sendTo);

            // Open the share contract with Mail only:

            await EmailManager.ShowComposeNewEmailAsync(mail);

    */

            Frame.Navigate(typeof(FeedbackPage));
        }

        private void LoadMore_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ((HubPageViewModel) this.DataContext).ShowAll = true;
        }

        private void FilterEvents_Clicked(object sender, RoutedEventArgs e)
        {
            if (((PivotItem) this.HubPivot.SelectedItem) == this.Attend)
            {
                if (this.ViewModel.CurrentFilterCriteria == null)
                {
                    Frame.Navigate(typeof (FilterEventsPage));
                }
                else
                {
                    Frame.Navigate(typeof(FilterEventsPage),
                        JsonConvert.SerializeObject(this.ViewModel.CurrentFilterCriteria));
                }
            }
            else if (((PivotItem) this.HubPivot.SelectedItem) == this.Learn)
            {
                if (this.ViewModel.CurrentLearningResourcesFilterCriteria == null)
                {
                    Frame.Navigate(typeof(FilterLearningResourcesPage));
                }
                else
                {
                    Frame.Navigate(typeof(FilterLearningResourcesPage),
                    JsonConvert.SerializeObject(this.ViewModel.CurrentLearningResourcesFilterCriteria));
                }
            }
                
              

        }

        public HubPageViewModel ViewModel
        {
            get { return this.DataContext as HubPageViewModel; }
        }

        private async void Reset_Filter_Clicked(object sender, RoutedEventArgs e)
        {

            if (((PivotItem) this.HubPivot.SelectedItem) == this.Attend)
            {

                await ((HubPageViewModel) this.DataContext).ResetFiltersEvent();
                //Frame.Navigate(typeof(HubPage),
                //    JsonConvert.SerializeObject(new ResetFilterPageNavigationParameters("")
                //    {
                //        Role = "",
                //        Location = "",
                //        Technology = ""
                //    }));
            }
            if (((PivotItem)this.HubPivot.SelectedItem) == this.Learn)
            {

                await ((HubPageViewModel)this.DataContext).ResetFiltersLearningResources();
                //Frame.Navigate(typeof(HubPage),
                //    JsonConvert.SerializeObject(new ResetFilterPageNavigationParameters("")
                //    {
                //        Role = "",
                //        Location = "",
                //        Technology = ""
                //    }));
            }
        }

        private async void Pivot_OnPivotItemLoaded(Pivot sender, PivotItemEventArgs args)
        {
            ((HubPageViewModel)this.DataContext).ShowAll = false;

            if (args.Item.Name == "MyZone")
            {
                var data = await LocalStorage.ReadJsonFromFile<List<TrackSpeaker>>("followedSpeakers");
                if (data == null || data.Count ==0)
                {
                    ((HubPageViewModel)this.DataContext).SpeakersTileList = false;
                    return;
                }

                //if(data.Count == 0)
                //{
                //    ((HubPageViewModel)this.DataContext).SpeakersTileList = false;
                //    return;
                //}

                ((HubPageViewModel)this.DataContext).SpeakersTileList = true;
                ((HubPageViewModel)this.DataContext).AddOnlyLimitedSpeakers(data, 8);
                //this.SpeakersTile.ItemsSource = data;
            }

            if (args.Item.Name == "Learn")
            {
                await ((HubPageViewModel)this.DataContext).GetLearningContent();
            }

            if( args.Item.Name == "Inbox")
            {
                 await ((HubPageViewModel)this.DataContext).GetNotifications();
            }
            if (args.Item.Name == "Attend")
            {
                await ((HubPageViewModel)this.DataContext).GetEvents();
            }
            
        }

        private void Speakers_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(SpeakersListPage));
        }

        private void SpeakersTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //var grid = sender as Grid;
            //if (grid == null)
            //{
            //    return;
            //}
            //var session = grid.Tag as TrackSession;
            //if (session == null)
            //{
            //    return;
            //}
            //Frame.Navigate(typeof(SessionDetailsPage), JsonConvert.SerializeObject(session));
            var grid = sender as Grid;
            if(grid == null)
            {
                return;
            }

            var speaker = grid.Tag as TrackSpeaker;
            if(speaker == null)
            {
                return;
            }
            Frame.Navigate(typeof(SpeakerDetailsPage), JsonConvert.SerializeObject(speaker));

        }
        private void Watched_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(WatchedVideosListPage));
        }

        private void Notification_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var grid = sender as Grid;
            if(grid == null)
            {
                return;
            }

            var notification = grid.Tag as Notification;

            if(notification == null)
            {
                return;
            }

            ((HubPageViewModel)this.DataContext).Notification_Tapped(notification);

            //if(notification.NotificationType == 3)
            //{
            if (!string.IsNullOrEmpty(notification.ActionLink))
            {
                Uri actionLink;
                if (Uri.TryCreate(notification.ActionLink, UriKind.Absolute, out actionLink))
                {
                    Frame.Navigate(typeof (EventRegistrationPage), notification.ActionLink);
                }
            }
            //}
        }

        private void NotificationGrid_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var datacontext = (e.OriginalSource as FrameworkElement).DataContext;
                var notification = datacontext as Notification;
                //service call on removed item from item and savedd it locally

                ((HubPageViewModel) this.DataContext).RemoveNotification_Click(notification);
            }
            catch
            {
            }

        }

        private void FavLearningResource_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Image sen = (sender as Image);
            if (sen != null)
            {
                var lr = ((sen.Parent as FrameworkElement).Parent as Grid).Tag as LearningResource;
                lr.Favourited = true;
                LearnPageViewModel.MarkVideos(lr);
            }


        }

        private void UnFavLearningResource_Tapped(object sender, TappedRoutedEventArgs e)
        {

            Image sen = (sender as Image);
            if (sen != null)
            {
                var lr = ((sen.Parent as FrameworkElement).Parent as Grid).Tag as LearningResource;
                lr.Favourited = false;
                LearnPageViewModel.MarkVideos(lr);
            }

        }
    }
}

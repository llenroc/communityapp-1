using TechReady.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using TechReady.Common.Models;
using TechReady.Helpers.MessageHelper;
using TechReady.Helpers.Storage;
using TechReady.ViewModels;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TechReady.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EventDetailsPage : Page
    {
        public EventDetailsPage()
        {
            this.InitializeComponent();
            this.DataContext = new EventDetailsPageViewModel();
        }

        public EventDetailsPageViewModel ViewModel
        {
            get
            {
                return this.DataContext as EventDetailsPageViewModel;
            }
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null)
            {

              
                    this.ViewModel.Event =
                         JsonConvert.DeserializeObject<Event>(e.Parameter.ToString());
                

                var data = await LocalStorage.ReadJsonFromFile<List<Event>>("followedEvents");
                if (data == null)
                {
                    data = new List<Event>();
                }

                var viewModel = this.DataContext as EventDetailsPageViewModel;
                if (viewModel == null)
                {
                    return;
                }

                if (data.FirstOrDefault(x => x.EventId == viewModel.Event.EventId) != null)
                {
                    viewModel.Event.IsFollowed = true;
                }
            }

            if (e.NavigationMode == NavigationMode.Back && Helpers.CommonSettings.FromRegisterationPage)
            {
                if (!((EventDetailsPageViewModel)this.DataContext).Event.IsFollowed)
                    this.PromptUserToFollow();
                Helpers.CommonSettings.FromRegisterationPage = false;
            }

            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested +=
            new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.DataRequested);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {

        }

        #endregion

        private void Register_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((EventDetailsPageViewModel) this.DataContext).Event.EventRegLink))
            {
                Helpers.CommonSettings.FromRegisterationPage = true; // flag 
                Frame.Navigate(typeof (EventRegistrationPage),
                    ((EventDetailsPageViewModel) this.DataContext).Event.EventRegLink);
            }
        }

        private async void PromptUserToFollow()
        {
            await Helpers.MessageHelper.MessageHelper.ShowMessage("Will you like to follow this event to receive updates?", "", "Ok", "Cancel", ContentDialog_PrimaryButtonClick);

            //MessageContentDialog msgDialog = new MessageContentDialog("Will you like to follow this event to receive updates?");
            //msgDialog.PrimaryButtonText = "Ok";
            //msgDialog.SecondaryButtonText = "Cancel";
            //msgDialog.PrimaryButtonClick += ContentDialog_PrimaryButtonClick;
            //await msgDialog.ShowMessage();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Follow_Click(sender, null);
        }

        private async void DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var viewModel = this.DataContext as EventDetailsPageViewModel;

            DataRequest request = args.Request;
            DataRequestDeferral deferral = request.GetDeferral();
            // Make sure we always call Complete on the deferral.
            try
            {
                request.Data.Properties.Title = "I am attending";
                request.Data.Properties.ApplicationName = "Tech Ready";

                if (!string.IsNullOrEmpty(viewModel.Event.EventRegLink))
                {
                    request.Data.SetText(string.Format("{0}, registration Link {1}",viewModel.Event.EventName, viewModel.Event.EventRegLink));
                }
            }
            finally
            {
                deferral.Complete();
            }
        }



        private void Share_Clicked(object sender, RoutedEventArgs e)
        {
            Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();

        }

        private  async void Follow_Click(object sender, RoutedEventArgs e)
        {
            //var data = await LocalStorage.ReadJsonFromFile<List<Event>>("followedEvents");
            //if (data == null)
            //{
            //    data = new List<Event>();
            //}

            var viewModel = this.DataContext as EventDetailsPageViewModel;
            if (viewModel == null)
            {
                return;
            }

            await ((EventDetailsPageViewModel)this.DataContext).Follow_Click(sender);

            //if (data.FirstOrDefault(x => x.EventId==viewModel.Event.EventId) ==null)
            //{
            //    data.Add(viewModel.Event);
            //    viewModel.Event.IsFollowed = true;
            //}
            //else
            //{
            //    data.Remove(viewModel.Event);
            //    viewModel.Event.IsFollowed = false;
            //}

            //await LocalStorage.SaveJsonToFile(data, "followedEvents");
        }


        private void Session_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var grid = sender as Grid;
            if (grid == null)
            {
                return;
            }
            var session = grid.Tag as TrackSession;
            if (session == null)
            {
                return;
            }
            Frame.Navigate(typeof(SessionDetailsPage), JsonConvert.SerializeObject(session));

            
        }

        private void Speaker_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var grid = sender as Grid;
            if(grid == null)
            {
                return;
            }
            var speaker = grid.Tag as TrackSpeaker;
            if (speaker == null)
            {
                return;
            }
            Frame.Navigate(typeof(SpeakerDetailsPage), JsonConvert.SerializeObject(speaker));
        }

        private static Dictionary<string,int> _selectedIndex = new Dictionary<string, int>();

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            var key = string.Format("{0}-{1}", this.ViewModel.Event.EventId, this.Frame.BackStackDepth);


            if (_selectedIndex != null)
            {
                if (e.NavigationMode == NavigationMode.Forward || e.NavigationMode == NavigationMode.New)
                {
                    if (!_selectedIndex.ContainsKey(key))
                    {
                        _selectedIndex.Add(key,this.EventTrackSelector.SelectedIndex);
                    }
                    else
                    {
                        _selectedIndex[key] = this.EventTrackSelector.SelectedIndex;
                    }
                }
                if(e.NavigationMode == NavigationMode.Back)
                {
                    if (_selectedIndex.ContainsKey(key))
                    {
                        _selectedIndex.Remove(key);
                    }
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var key = string.Format("{0}-{1}", this.ViewModel.Event.EventId, this.Frame.BackStackDepth);


            if (this.ViewModel.HasTracks && this.ViewModel.Event.Tracks.Count > 0 && this.EventTrackSelector.SelectedIndex == -1)
            {

                if (_selectedIndex.ContainsKey(key))
                {
                    this.EventTrackSelector.SelectedItem = this.ViewModel.Event.Tracks[_selectedIndex[key]];
                }
                else
                {
                    this.EventTrackSelector.SelectedItem = this.ViewModel.Event.CurrentTrack;
                }
                
                //this.EventTrackSelector.SelectedItem = this.ViewModel.Event.CurrentTrack;
            }

        }
    }
}

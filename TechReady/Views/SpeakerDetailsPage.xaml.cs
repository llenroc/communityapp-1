using TechReady.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TechReady.ViewModels;
using Newtonsoft.Json;
using TechReady.Common.Models;
using TechReady.Helpers.Storage;
using TechReady.Models;
using System.Diagnostics;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TechReady.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SpeakerDetailsPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

      //  private SpeakerDetailsPageViewModel _speakerDetailsPageViewModel = new SpeakerDetailsPageViewModel();

        public SpeakerDetailsPage()
        {
            this.InitializeComponent();
            this.DataContext = new SpeakerDetailsPageViewModel();
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
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
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait; //Restricting Rotation

                Pivot.SelectedIndex = PivotSelectedIndex;
            if (e.NavigationMode == NavigationMode.Back)
            {
                switch (Pivot.SelectedIndex)
                {
                    case 0:
                        break;
                    case 1:
                        ((SpeakerDetailsPageViewModel)this.DataContext).GetBlogsForSpeaker();
                        break;
                    case 2:
                        ((SpeakerDetailsPageViewModel)this.DataContext).GetUpcommingSessions();
                        break;
                    default: 
                        break;
                }
            }

            if (e.Parameter != null)
            {
                var trackSpeaker = JsonConvert.DeserializeObject<TrackSpeaker>(e.Parameter.ToString());

                if (trackSpeaker != null)
                {
                    //_speakerDetailsPageViewModel.Speaker = trackSpeaker;
                    //this.DataContext = _speakerDetailsPageViewModel;
                    //_speakerDetailsPageViewModel.GetSpeakerDetails(trackSpeaker);
                    ((SpeakerDetailsPageViewModel)this.DataContext).Speaker = trackSpeaker;
                    await ((SpeakerDetailsPageViewModel)this.DataContext).GetSpeakerDetails(trackSpeaker);
                    ((SpeakerDetailsPageViewModel)this.DataContext).GetBlogsForSpeaker();
                }


                var data = await LocalStorage.ReadJsonFromFile<List<TrackSpeaker>>("followedSpeakers");
                if (data == null)
                {
                    data = new List<TrackSpeaker>();
                }

                var viewModel = this.DataContext as SpeakerDetailsPageViewModel;
                if (viewModel == null)
                {
                    return;
                }

                if (data.FirstOrDefault(x => x.SpeakerId == viewModel.Speaker.SpeakerId) != null)
                {
                    viewModel.Speaker.IsFollowed = true;
                }
            }

            //if (e.NavigationMode == NavigationMode.Back || e.NavigationMode == NavigationMode.Refresh)
            //{
            //    ((SpeakerDetailsPageViewModel)this.DataContext).GetUpcommingSessions();
            //    ((SpeakerDetailsPageViewModel)this.DataContext).GetBlogsForSpeaker();
            //}
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;  //Removing Orientation Preferences
            base.OnNavigatedFrom(e);
        }

        private static Dictionary<string, int> _pivotSelectedIndex = new Dictionary<string, int>();   //local variable to keep record of the pivot..

        public int PivotSelectedIndex
        {
            get
            {
                if (((SpeakerDetailsPageViewModel)this.DataContext) != null && ((SpeakerDetailsPageViewModel)this.DataContext).Speaker != null)
                {
                    var key = string.Format("{0}", this.Frame.BackStackDepth);
                    if (_pivotSelectedIndex.ContainsKey(key))
                    {
                        int i = _pivotSelectedIndex[key];
                        _pivotSelectedIndex.Remove(key);
                        return i;
                    }
                }
                return 0;
            }

            set
            {
                if (((SpeakerDetailsPageViewModel)this.DataContext) != null && ((SpeakerDetailsPageViewModel)this.DataContext).Speaker != null)
                {
                    var key = string.Format("{0}",this.Frame.BackStackDepth);
                    if (!_pivotSelectedIndex.ContainsKey(key))
                    {
                        _pivotSelectedIndex.Add(key, value);
                    }
                }
            }
        }

        private void PivotItemLoaded(Pivot sender, PivotItemEventArgs args)
        {
            CommandBar.Visibility = Visibility.Collapsed;
            //((SpeakerDetailsPageViewModel) this.DataContext).IsBlogsAvailable = true;

            if (args.Item == ProfilePivot)
            {
                CommandBar.Visibility = Visibility.Visible;

                //((SpeakerDetailsPageViewModel)this.DataContext).IsBlogsAvailable = true;
                //BlogsErrorTextBlock.Visibility = Visibility.Collapsed;
            }
            else if (args.Item == BlogPivot)
            {
                ((SpeakerDetailsPageViewModel) this.DataContext).GetBlogsForSpeaker();
            }

            else if (args.Item == UpcomingSessionPivot)
            {
                ((SpeakerDetailsPageViewModel) this.DataContext).GetUpcommingSessions();
            }
        }


        #endregion

        private void Blog_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var StackPanel = sender as StackPanel;
            if (StackPanel == null)
            {
                return;
            }
            var lr =
            StackPanel.Tag as LearningResource;

            if (lr == null)
            {
                return;
            }
            this.PivotSelectedIndex = 1;   //local variable to keep record of the pivot..
            Frame.Navigate(typeof(EventRegistrationPage), lr.Link);
        }

      
        private async void Follow_Click(object sender, RoutedEventArgs e)
        {
            //var data = await LocalStorage.ReadJsonFromFile<List<TrackSpeaker>>("followedSpeakers");
            //if (data == null)
            //{
            //    data = new List<TrackSpeaker>();
            //}
            var viewModel = this.DataContext as SpeakerDetailsPageViewModel;
            if (viewModel == null)
            {
                return;
            }

            ((SpeakerDetailsPageViewModel) this.DataContext).Follow_Click(sender);
          //if(data.FirstOrDefault( x => x.SpeakerId == viewModel.Speaker.SpeakerId) == null)
          //  {
          //      data.Add(viewModel.Speaker);
          //      viewModel.Speaker.IsFollowed = true;
          //  }
          //  else
          //  {
          //      data.Remove(viewModel.Speaker);
          //      viewModel.Speaker.IsFollowed = false;
          //  }


          //  await LocalStorage.SaveJsonToFile(data, "followedSpeakers");

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
            this.PivotSelectedIndex = 2;   //local variable to keep record of the pivot..
            Frame.Navigate(typeof(EventDetailsPage), JsonConvert.SerializeObject(ev));

        }

        private void SocialLink_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try {
                var stackpanel = sender as StackPanel;
                if (stackpanel == null)
                {
                    return;
                }

                var speaker = stackpanel.Tag as SpeakerDetailsPageViewModel;

                this.PivotSelectedIndex = 0;   //local variable to keep record of the pivot..
                if (stackpanel.Name == "Twitter")
                {
                    var link = "https://twitter.com/" + speaker.Speaker.TwitterLink.Substring(1);
                    Frame.Navigate(typeof(EventRegistrationPage), link);
                }

                if (stackpanel.Name == "Linkedin")
                {
                    var link = speaker.Speaker.LinkedinLink;
                    Frame.Navigate(typeof(EventRegistrationPage), link);
                }

                ////var speaker = stackpanel.Tag as SpeakerDetailsPageViewModel;
                ////var link = speaker.Speaker.TwitterLink;
                //if(speaker == null)
                //{
                //    return;
                //}
                //Frame.Navigate(typeof(EventRegistrationPage),link);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Exception thrown at SocialLink_Tapped " + ex.Message);
            }
        }
    }
}

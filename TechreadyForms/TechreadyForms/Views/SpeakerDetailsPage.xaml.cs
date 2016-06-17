using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TechReady.Common.Models;
using TechReady.Helpers.Storage;
using TechReady.Models;
using TechReady.ViewModels;
using Xamarin.Forms;

namespace TechreadyForms.Views
{
    public partial class SpeakerDetailsPage : TabbedPage
    {
        private TrackSpeaker _paramter;
        private ToolbarItem followToolbarItem;

        private ToolbarItem unFollowToolbarItem;
        public SpeakerDetailsPage(TrackSpeaker parameter)
        {
            this._paramter = parameter;
            this.BindingContext = new SpeakerDetailsPageViewModel();

            followToolbarItem = new ToolbarItem("Follow", "Follow.png", () =>
            {
                this.Follow_Click(this, new EventArgs());
            }
           );

            unFollowToolbarItem = new ToolbarItem("Unfollow", "UnFollow.png", () =>
            {
                this.Follow_Click(this, new EventArgs());
            }
                );

            InitializeComponent();

        }

        private SpeakerDetailsPageViewModel ViewModel
        {
            get { return this.BindingContext as SpeakerDetailsPageViewModel; }
        }

        private void Follow_Click(object sender, EventArgs e)
        {
            this.ViewModel.Follow_Click(sender);
        }


        private async void SpeakerDetailsPage_OnAppearing(object sender, EventArgs e)
        {
            //TODO: Write code to show the right Tab Page when user navigates back. 
            //Pivot.SelectedIndex = PivotSelectedIndex;
            //if (e.NavigationMode == NavigationMode.Back)
            //{
            //    switch (Pivot.SelectedIndex)
            //    {
            //        case 0:
            //            break;
            //        case 1:
            //            ((SpeakerDetailsPageViewModel)this.DataContext).GetBlogsForSpeaker();
            //            break;
            //        case 2:
            //            ((SpeakerDetailsPageViewModel)this.DataContext).GetUpcommingSessions();
            //            break;
            //        default:
            //            break;
            //    }
            //}
            if (_paramter != null)
            {
                var trackSpeaker = _paramter;

                if (trackSpeaker != null)
                {
                    //_speakerDetailsPageViewModel.Speaker = trackSpeaker;
                    //this.DataContext = _speakerDetailsPageViewModel;
                    //_speakerDetailsPageViewModel.GetSpeakerDetails(trackSpeaker);
                    this.ViewModel.Speaker = trackSpeaker;

                    this.ViewModel.Speaker.FollowedChanged += SpeakerOnFollowedChanged;

                    if (this.ViewModel.Speaker == null)
                        await this.ViewModel.GetSpeakerDetails(trackSpeaker);



                    var data = await LocalStorage.ReadJsonFromFile<List<TrackSpeaker>>("followedSpeakers");
                    if (data == null)
                    {
                        data = new List<TrackSpeaker>();
                    }


                    if (this.ViewModel != null &&
                        data.FirstOrDefault(x => x.SpeakerId == this.ViewModel.Speaker.SpeakerId) != null)
                    {
                        this.ViewModel.Speaker.IsFollowed = true;
                    }
                    else
                    {
                        this.ViewModel.Speaker.IsFollowed = false;
                    }
                }
            }
        }

        private void SpeakerOnFollowedChanged()
        {
            this.ToolbarItems.Clear();
            if (this.ViewModel.Speaker.IsFollowed)
            {
                this.ToolbarItems.Add(unFollowToolbarItem);

            }
            else
            {
                this.ToolbarItems.Add(followToolbarItem);
            }
        }

        private  void Blogs_Appearing(object sender, EventArgs e)
        {
            if (this.ViewModel.Speaker.Blogs==null)
            {
                this.ViewModel.GetBlogsForSpeaker();
            }
            else
            {
                this.ViewModel.OnPropertyChanged("");
                this.ViewModel.Speaker.OnPropertyChanged("");
            }

        }

        private void UpcomingSessions_Appearing(object sender, EventArgs e)
        {
            if (this.ViewModel.Speaker.SpeakerEvents == null)
            {
                this.ViewModel.GetUpcommingSessions();
            }
        }


        private void Blog_Selected(object sender, ItemTappedEventArgs e)
        {
           
            if (e.Item == null)
            {
                return;
            }
            if (e.Item is LearningResource)
            {
                Navigation.PushAsync(new WebViewPage(((LearningResource)e.Item).Link));
            }
            //this.PivotSelectedIndex = 1;   //local variable to keep record of the pivot..
            this.BlogsList.SelectedItem = null;
        }

        //private static Dictionary<string, int> _pivotSelectedIndex = new Dictionary<string, int>();   //local variable to keep record of the pivot..

        //public int PivotSelectedIndex
        //{
        //    get
        //    {
        //        if ((this.ViewModel != null && (this.ViewModel.Speaker != null)))
        //        {
        //            var key = string.Format("{0}", Navigation.NavigationStack.Count);
        //            if (_pivotSelectedIndex.ContainsKey(key))
        //            {
        //                int i = _pivotSelectedIndex[key];
        //                _pivotSelectedIndex.Remove(key);
        //                return i;
        //            }
        //        }
        //        return 0;
        //    }

        //    set
        //    {
        //        if ((this.ViewModel != null && (this.ViewModel.Speaker != null)))
        //        {
        //            var key = string.Format("{0}", Navigation.NavigationStack.Count);
        //            if (!_pivotSelectedIndex.ContainsKey(key))
        //            {
        //                _pivotSelectedIndex.Add(key, value);
        //            }
        //        }
        //    }
        //}

        private void Event_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event

            if (e.Item is Event)
            {
                Navigation.PushAsync(new EventDetails((Event)e.Item));
            }
            ((ListView) sender).SelectedItem = null; // de-select the row
        }

        private void Twitter_Tapped(object sender, EventArgs e)
        {
            var link = "https://twitter.com/" + this.ViewModel.Speaker.TwitterLink.Substring(1);
            Navigation.PushAsync(new WebViewPage(link));
        }

        private void LinkedIn_Tapped(object sender, EventArgs e)
        {
            var link = this.ViewModel.Speaker.LinkedinLink;
            Navigation.PushAsync(new WebViewPage(link));

        }
    }
}

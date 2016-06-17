using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TechReady.Common.DTO;
using TechReady.Common.Models;
using TechReady.Helpers.MessageHelper;
using TechReady.Helpers.ServiceCallers;
using TechReady.Helpers.Storage;

namespace TechReady.ViewModels
{
    public class EventDetailsPageViewModel :BaseViewModel
    {
        private Event _event;

        public Event Event
        {
            get
            {
                return _event;
            }
            set
            {
                if (this._event != value)
                {
                    this._event = value;
                    OnPropertyChanged("Event");
                    OnPropertyChanged("Tracks");
                    OnPropertyChanged("HasTracks");
                    OnPropertyChanged("CurrentTrackName");
                    OnPropertyChanged("HasRegisterationLink");
                    OnPropertyChanged("HasEventAbstract");
                }
            }
        }
        
        public bool HasRegisterationLink
        {
            get
            {
                if (this._event != null && !string.IsNullOrEmpty(this._event.EventRegLink))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool HasEventAbstract
        {
            get
            {
                if (this._event != null && !string.IsNullOrEmpty(this._event.EventAbstract))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool HasTracks
        {
            get
            {
                if(this.Event!=null && this.Event.Tracks!=null && this.Event.Tracks.Count > 0)
                    return true;
                return false;
            }
        }

        public string CurrentTrackName
        {
            get
            {
                if (Event!=null && Event.CurrentTrack != null)
                {
                    return Event.CurrentTrack.TrackDisplayName;
                }
                return "";
            }
            set
            {
                if (Event==null || Event.Tracks == null)
                {
                    return;
                }
                var track = Event.Tracks.FirstOrDefault(x => x.TrackDisplayName == value);
                {
                    Event.CurrentTrack = track;
                    OnPropertyChanged("CurrentTrackName");
                }
            }
        }

       
        public event Action FollowedChanged;
        public async Task Follow_Click(object sender)
        {
            try {
                var data = await LocalStorage.ReadJsonFromFile<List<Event>>("followedEvents");
                if (data == null)
                {
                    data = new List<Event>();
                }
                //var viewModel = this.DataContext as EventDetailsPageViewModel;
                //if (viewModel == null)
                //{
                //    return;
                //}

                if (data.FirstOrDefault(x => x.EventId == this.Event.EventId) == null)
                {
                    data.Add(this.Event);
                    this.Event.IsFollowed = true;
                }
                else
                {
                    data.Remove(this.Event);
                    this.Event.IsFollowed = false;
                }
                var model = await LocalStorage.ReadJsonFromFile<UserRegistrationPageViewModel>("registration");
                if (model != null)
                {
                    if (Helpers.NetworkHelper.NetworkHelper.IsNetworkAvailable() == true)
                    {
                        //TODO SHIV: API Check
                        var result = await ServiceProxy.CallService("api/FollowEvent", JsonConvert.SerializeObject(new TechReady.Common.DTO.FollowEventRequest()
                        {
                            EventId = Event.EventId.ToString(),
                            AppUserId = model.UserId,
                            Follow = Event.IsFollowed
                        }));

                        if (result.IsSuccess)
                        {
                            model.SaveUserRegitration();
                        }
                    }
                }
                await LocalStorage.SaveJsonToFile(data, "followedEvents");

                if (FollowedChanged != null)
                {
                    this.FollowedChanged();
                }
            }catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception thrown at EventDetailsPage view model "+ex.Message);
            }

        }
    }
}
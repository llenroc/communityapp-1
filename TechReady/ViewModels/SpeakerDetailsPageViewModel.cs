using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechReady.API.DTO;
using TechReady.Common.DTO;
using TechReady.Common.Models;
using TechReady.Helpers.NetworkHelper;
using TechReady.Helpers.ServiceCallers;
using TechReady.Helpers.Storage;
using TechReady.ServiceCallers;

namespace TechReady.ViewModels
{
   public class SpeakerDetailsPageViewModel :BaseViewModel
    {
        private TrackSpeaker _speaker;

        public TrackSpeaker Speaker
        {
            get
            {
                return _speaker;
            }
            set
            {
                if (this._speaker != value)
                {
                    this._speaker = value;
                    OnPropertyChanged("Speaker");
                    OnPropertyChanged("IsLocationAvailable");
                    OnPropertyChanged("IsTwitterLinkAvailable");
                    OnPropertyChanged("IsFacebookLinkAvailable");
                    OnPropertyChanged("IsLinkedinLinkAvailable");
                    OnPropertyChanged("IsBlogsAvailable");
                    OnPropertyChanged("IsSessionsAvailable");
                    if (_speaker != null)
                    {
                        _speaker.NotifyPropertyChanges();
                    }
                }
            }
        }

        public bool IsLocationAvailable
        {
            get
            {
                return ( this.Speaker!=null && !string.IsNullOrEmpty(this.Speaker.Location) );
            }
        }
        public bool IsTwitterLinkAvailable
        {
            get
            {
                return (this.Speaker != null && !string.IsNullOrEmpty(this.Speaker.TwitterLink)) ;
            }

        }
        public bool IsFacebookLinkAvailable
        {
            get
            {
                return (this.Speaker != null && !string.IsNullOrEmpty(this.Speaker.FacebookLink));
            }
            
        }
        public bool IsLinkedinLinkAvailable
        {
            get
            {
                return (this.Speaker != null && !string.IsNullOrEmpty(this.Speaker.LinkedinLink));
            }

        }

     
        public bool IsBlogsAvailable
        {
            get
            {
                if (this.Speaker != null && this.Speaker.Blogs != null && this.Speaker.Blogs.Count > 0)
                {
                    return true;
                }

                return false;
            }
     
     
        }

        private bool _isSessionsAvailable = true;
        public bool IsSessionsAvailable
        {
            get
            {
                return this.Speaker != null && this.Speaker.SpeakerEvents != null && Speaker.SpeakerEvents.Count > 0;
            }
          
        }



        public async void GetBlogsForSpeaker()
        {
            try
            {
                if (this.Speaker == null)
                {
                    return;
                }
                if (NetworkHelper.IsNetworkAvailable() == false)
                {
                    return;
                }

                this.OperationInProgress = true;

                this.Speaker.Blogs = await BlogService.GetBlogs(this.Speaker.BlogLink);
                OnPropertyChanged("Speaker");
                OnPropertyChanged("Blogs");
                OnPropertyChanged("IsBlogsAvailable");
                //if (this.Speaker.Blogs != null && this.Speaker.Blogs.Count > 0)
                //{
                //    this.IsBlogsAvailable = true;
                //}
                //else
                //    this.IsBlogsAvailable = false;
            }
            catch
            {
                OnPropertyChanged("");
            }
            finally
            {
                this.OperationInProgress = false;
                OnPropertyChanged("");
            }
        }


        //public async void GetUpcomingSession()
        //{
        //    if(this.Speaker == null)
        //    {
        //        return;
        //    }
        //    if(NetworkHelper.IsNetworkAvailable() == false)
        //    {
        //        return;
        //    }
        //    this.OperationInProgress = true;
        //    this.Speaker.SpeakerEvents = await EventService.GetService(this.Speaker.SessionList);
        //    this.OperationInProgress = false;
        //}

        public async Task GetSpeakerDetails(TrackSpeaker args)
        {
            var trackSpeaker = args;
            if (NetworkHelper.IsNetworkAvailable() != false)
            {
                try
                {
                    this.OperationInProgress = true;

                    SingleSpeakerRequest request = new SingleSpeakerRequest()
                    {
                        SpeakerId = trackSpeaker.SpeakerId
                    };
                    
                    var result = await ServiceProxy.CallService("api/SingleSpeaker", JsonConvert.SerializeObject(request));


                    if (result.IsSuccess)
                    {
                        var speakerDetails = JsonConvert.DeserializeObject<SingleSpeakerResponse>(result.response);
                        if (speakerDetails.TrackSpeaker != null)
                        {
                            trackSpeaker = speakerDetails.TrackSpeaker;
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Exception on: SpeakerDetailsPageVM.GetSpeakerDetails(): ExceptionMessage: " + e.Message);
                    OnPropertyChanged("");
                }
                finally
                {
                    this.OperationInProgress = false;
                OnPropertyChanged("");
                }
            }

            this.Speaker = trackSpeaker;
            OnPropertyChanged("Speaker");
        }

        public async void GetUpcommingSessions()
       {
           if (NetworkHelper.IsNetworkAvailable() == false)
           {
               return;
           }
           try
           {

               this.OperationInProgress = true;

               SingleSpeakerRequest request = new SingleSpeakerRequest()
               {
                   SpeakerId = this.Speaker.SpeakerId
               };

               var result = await ServiceProxy.CallService("api/SpeakerEvents", JsonConvert.SerializeObject(request));


               if (result.IsSuccess)
               {
                   var speakerEventsResponse = JsonConvert.DeserializeObject<SpeakerEventsResponse>(result.response);
                   if (speakerEventsResponse.SpeakerEvents != null)
                   {
                       Speaker.SpeakerEvents = this.FilterEvents(speakerEventsResponse.SpeakerEvents);
                        OnPropertyChanged("Speaker");
                        OnPropertyChanged("SpeakerEvents");
                        OnPropertyChanged("IsSessionsAvailable");
                    }
               }
                
           }
           catch (Exception e)
           {
               System.Diagnostics.Debug.WriteLine(
                   "Exception on: SpeakerDetailsPageVM.GetSpeakerDetails(): ExceptionMessage: " + e.Message);
                OnPropertyChanged("");
            }
           finally
           {
               //if (Speaker.SpeakerEvents != null && Speaker.SpeakerEvents.Count > 0)
               //{
               //    IsSessionsAvailable = true;
               //}
               //else
               //{
               //    IsSessionsAvailable = false;
               //}
                this.OperationInProgress = false;
                OnPropertyChanged("");
           }
        }
        public async void Follow_Click(object sender)
        {
            try {
                var data = await LocalStorage.ReadJsonFromFile<List<TrackSpeaker>>("followedSpeakers");
                if (data == null)
                {
                    data = new List<TrackSpeaker>();
                }

                //var viewModel = this.DataContext as SpeakerDetailsPageViewModel;
                //if (viewModel == null)
                //{
                //    return;
                //}

                if (data.FirstOrDefault(x => x.SpeakerId == this.Speaker.SpeakerId) == null)
                {
                    data.Add(this.Speaker);
                    this.Speaker.IsFollowed = true;
                }
                else
                {
                    data.Remove(this.Speaker);
                    this.Speaker.IsFollowed = false;
                }

                var model = await LocalStorage.ReadJsonFromFile<UserRegistrationPageViewModel>("registration");
                if (model != null)
                {
                    if (Helpers.NetworkHelper.NetworkHelper.IsNetworkAvailable() == true)
                    {
                        //TODO SHIV: API Check
                        var result = await ServiceProxy.CallService("api/FollowSpeaker", JsonConvert.SerializeObject(new TechReady.Common.DTO.FollowSpeakerRequest()
                        {
                            SpeakerId = Speaker.SpeakerId.ToString(),
                            AppUserId = model.UserId,
                            Follow = Speaker.IsFollowed
                        }));

                        if (result.IsSuccess)
                        {
                            model.SaveUserRegitration();
                        }
                    }
                }
                await LocalStorage.SaveJsonToFile(data, "followedSpeakers");
            }catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception thrown at SpeakerDetailsPage view model " + ex.Message);
                OnPropertyChanged("");
            }
        }

        public ObservableCollection<Event> FilterEvents(ObservableCollection<Event> list)
        {
            try
            {
                if (list != null && list.Count > 0)
                {
                    var result = list.OrderBy(x => x.EventFromDate);
                    return new ObservableCollection<Event>(result);
                }
                return list;
            }
            catch
            {
                return list;
            }
        }
    }
}

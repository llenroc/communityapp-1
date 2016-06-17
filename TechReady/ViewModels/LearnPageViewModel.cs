using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TechReady.Common.DTO;
using TechReady.Common.Models;
using TechReady.Helpers.ServiceCallers;
using TechReady.Helpers.Storage;
using TechReady.Models;

namespace TechReady.ViewModels
{
    public class LearnPageViewModel : BaseViewModel
    {
        public LearnPageViewModel()
        {
        }

        public async void Watched_Videos(object sender)
        {
            MarkVideos(sender);
        }


        public async static void MarkVideos(object sender)
        {
            try
            {
                var data = await LocalStorage.ReadJsonFromFile<List<LearningResource>>("watchedVideos");
                if (data == null)
                {
                    data = new List<LearningResource>();
                }

                var resource = sender as LearningResource;
                if (resource == null)
                {
                    return;
                }

                if (data.FirstOrDefault(x => x.LearningResourceID == resource.LearningResourceID) == null)
                {
                    resource.LastWatchedTime = DateTime.Now;
                    data.Add(resource);
                }
                else
                {
                    data.Remove(resource);
                    resource.LastWatchedTime = DateTime.Now;
                    //data.Add(resource);
                }

                var model = await LocalStorage.ReadJsonFromFile<UserRegistrationPageViewModel>("registration");
                if (model != null)
                {
                    if (Helpers.NetworkHelper.NetworkHelper.IsNetworkAvailable() == true)
                    {
                        //TODO SHIV: API Check
                        var result = await ServiceProxy.CallService("api/WatchedLearningResource", JsonConvert.SerializeObject(new TechReady.Common.DTO.WatchedLearningResourceRequest()
                        {
                            AppUserId = model.UserId,
                            WatchedLearningResourceId = resource.LearningResourceID.ToString()
                        }));

                        if (result.IsSuccess)
                        {
                            model.SaveUserRegitration();
                        }
                        //if (result.IsSuccess)
                        //{
                        //    //var homeResponse = JsonConvert.DeserializeObject<SpeakersResponse>(result.response);
                        //    //this.Speakers = homeResponse.AllSpeakers;
                        //    //this._allSpeakers = new ObservableCollection<TrackSpeaker>();
                        //    //foreach (var speaker in this.Speakers)
                        //    //{
                        //    //    _allSpeakers.Add(speaker);
                        //    //}
                        //    //await LocalStorage.SaveJsonToFile(this.Speakers, "allspeakers");
                        //}
                    }
                }

                await LocalStorage.SaveJsonToFile(data, "watchedVideos");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception thrown at LearnPage view Model " + ex.Message);
            }
        }

        //private ObservableCollection<Event> _allEvents;

        //public ObservableCollection<Event> AllEvents
        //{
        //    get
        //    {
        //        return _allEvents;
        //    }
        //    set
        //    {
        //        if (this._allEvents != value)
        //        {
        //            this._allEvents = value;
        //            OnPropertyChanged("AllEvents");
        //        }
        //    }
        //}

        //public async void GetFollowedEvents()
        //{
        //    var data = await LocalStorage.ReadJsonFromFile<List<Event>>("followedEvents");
        //    if (data == null)
        //    {
        //        data = new List<Event>();
        //    }

        //    this.AllEvents = new ObservableCollection<Event>(data);

        //}
    }
}
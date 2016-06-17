using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TechReady.Common.Models;
using TechReady.Helpers.Storage;

namespace TechReady.ViewModels
{
    public class FollowedSpeakersViewModel : BaseViewModel
    {
     
        private ObservableCollection<TrackSpeaker> _speakers;

        public ObservableCollection<TrackSpeaker> Speakers
        {
            get
            {
                return _speakers;
            }
            set
            {
                if (this._speakers != value)
                {
                    this._speakers = value;
                    OnPropertyChanged("Speakers");
                }
            }
        }

        
        public async Task GetFollowedSpeakers()
        {
            var data = await LocalStorage.ReadJsonFromFile<List<TrackSpeaker>>("followedSpeakers");
            if (data == null)
            {
                data = new List<TrackSpeaker>();
            }

            this.Speakers = new ObservableCollection<TrackSpeaker>(data);

        }
    }
}
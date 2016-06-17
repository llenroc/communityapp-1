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
using TechReady.Helpers.MessageHelper;
using TechReady.Helpers.NetworkHelper;
using TechReady.Helpers.ServiceCallers;
using TechReady.Helpers.Storage;

namespace TechReady.ViewModels
{
    public class SpeakersListPageViewModel : BaseViewModel
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
                    OnPropertyChanged("AreSpeakersAvailable");
                }
            }

        }

        

        public bool AreSpeakersAvailable
        {
            get
            {
                return (this.Speakers != null && this.Speakers.Count > 0);
            }
        }

        public async Task GetSpeakers()
        {
            this.OperationInProgress = true;
            try
            {
                ObservableCollection<TrackSpeaker> model;
                model = await LocalStorage.ReadJsonFromFile<ObservableCollection<TrackSpeaker>>("allspeakers");

                if (NetworkHelper.IsNetworkAvailable())
                {
                    SpeakersRequest request = new SpeakersRequest()
                    {

                    };

                    var result = await ServiceProxy.CallService("api/Speakers", JsonConvert.SerializeObject(request));

                    if (result.IsSuccess)
                    {
                        var homeResponse = JsonConvert.DeserializeObject<SpeakersResponse>(result.response);
                        await LocalStorage.SaveJsonToFile(this.Speakers, "allspeakers");
                        model = homeResponse.AllSpeakers;
                    }
                 
                }

                if (model != null)
                {
                    this.Speakers =
                        model;
                    _allSpeakers = model;
                }
            }
            finally
            {
                this.OperationInProgress = false;
            }
            
        }

        private ObservableCollection<TrackSpeaker> _allSpeakers;

        private bool _searchOpen = false;
        public bool SearchOpen
        {
            get
            {
                return _searchOpen;
            }
            set
            {
                if(this._searchOpen != value)
                {
                    this._searchOpen = value;
                    OnPropertyChanged("SearchOpen");
                }
            }
        }

        public void Search_Speakers(string query)
        {
            try {
                if (this._allSpeakers != null)
                {
                    var result = this._allSpeakers.Where(x => x.SpeakerName.ToLower().Contains(query.ToLower()));
                    this.Speakers = new ObservableCollection<TrackSpeaker>(result);
                }
            }
            catch { }
        }
    }
}

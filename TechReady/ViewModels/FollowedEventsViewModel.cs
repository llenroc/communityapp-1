using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TechReady.Common.Models;
using TechReady.Helpers.Storage;

namespace TechReady.ViewModels
{
    public class FollowedEventsViewModel : BaseViewModel
    {
        public FollowedEventsViewModel()
        {
        }

        private ObservableCollection<Event> _allEvents;

        public ObservableCollection<Event> AllEvents
        {
            get
            {
                return _allEvents;
            }
            set
            {
                if (this._allEvents != value)
                {
                    this._allEvents = value;
                    OnPropertyChanged("AllEvents");
                    OnPropertyChanged("AllShownEvents");
                }
            }
        }
        
        public ObservableCollection<Event> AllShownEvents
        {
            get
            {
                return this.FilterEvents(AllEvents);
            }
        }
        public async Task GetFollowedEvents()
        {
            var data = await LocalStorage.ReadJsonFromFile<List<Event>>("followedEvents");
            if (data == null)
            {
                data = new List<Event>();
            }

            this.AllEvents = new ObservableCollection<Event>(data);

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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechReady.Common.Models;

namespace TechReady.API.DTO
{
    public class SpeakersResponse
    {
        public ObservableCollection<TrackSpeaker> AllSpeakers { get; set; }
    }


    public class SingleSpeakerResponse
    {
        public TrackSpeaker TrackSpeaker { get; set; }
    }

    public class SpeakerEventsResponse
    {
        public ObservableCollection<Event> SpeakerEvents { get; set; }
    }
}

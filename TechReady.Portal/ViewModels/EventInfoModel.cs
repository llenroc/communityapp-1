using System.Collections.Generic;
using TechReady.Portal.Models;

namespace TechReady.Portal.ViewModels
{
    public class EventInfoModel
    {
        public Event Event { get; set; }
        public List<EventTrack> EventTracks { get; set; }
        public List<PrimaryTechnology> EventTechnologTags { get; set; }

        public List<AudienceType> EventAudienceTypeTags { get; set; }
    }
}
using System;
using System.Collections.Generic;
using TechReady.Portal.Controllers;
using TechReady.Portal.Models;

namespace TechReady.Portal.ViewModels
{
    public class EventViewModels
    {
        public List<City> AvailableCities { get; set; }

        public List<KeyValueObject> ScEligibility { get; set; }

        public List<KeyValueObject> EventStatus { get; set; }
        public List<KeyValueObject> EventVisibility { get; set; }
        public List<KeyValueObject> EventType { get; set; }
        public List<Speaker> Speakers { get; set; }
        public List<Object> Tracks { get;set; }
        public List<PrimaryTechnology> EventTechnologTags { get; set; }
        public List<AudienceType> EventAudienceTypeTags { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;

namespace TechReady.Portal.Models
{

    public class AppUser
    {
        public int AppUserID { get; set; }


        public string FullName { get; set; }


        public string Email { get; set; }


        public string Town { get; set; }

        public DbGeography Location { get; set; }

        public string CityName { get; set; }

        public virtual  City City { get; set; }

        public int AudienceOrgID { get; set; }

        public virtual AudienceOrg AudienceOrg { get; set; }




        public string AuthProviderUserId { get; set; }

        public string AuthProviderName { get; set; }

        public string DevicePlatform { get; set; }

        public DateTime? RegistrationDateTime { get; set; }

        public DateTime? LastAccessTime { get; set; }

        public bool PushEnabled { get; set; }

        public string DeviceId { get; set; }

        public string PushId { get; set; }

        public virtual ICollection<PrimaryTechnology> TechnologyTags { get; set; }

        public virtual ICollection<Event> FollowedEvents { get; set; }

        public virtual ICollection<Speaker> FollowedSpeakers { get; set; }

        public virtual ICollection<LearningResource> WatchedLearningResources { get; set; }

        public virtual ICollection<AppUserNotificationAction> NotificationsRecieved { get; set; }
        
    }
}
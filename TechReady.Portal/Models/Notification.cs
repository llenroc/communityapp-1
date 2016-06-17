using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechReady.Portal.Models
{
    public enum NotificationType
    {
        NewEvent,
        EventReminder,
        Announcement,
        Link,
        PreEvent,
        PostEvent
    }

    public class AppUserNotificationAction
    {
        public int AppUserNotificationActionID { get; set;}

        public bool Read { get; set; }

        public int  NotificationID { get; set; }
        public virtual Notification Notification { get; set; }


        public int AppUserID { get; set; }
        public virtual AppUser AppUser { get; set; }
        public bool Removed { get; set; }
    }

    public class Notification
    {
        public int NotificationID { get; set; }

        public NotificationType TypeOfNotification { get; set; }

        public int? ResourceId { get; set; }

        public string ActionLink { get; set; }
        [Required]
        public string NotificationTitle { get; set; }

        public string NotificationMessage { get; set; }

        public DateTime PushDateTime { get; set; }

        public virtual ICollection<AppUserNotificationAction> SentToUsers { get; set; }

        public virtual ICollection<PrimaryTechnology> TechTags { get; set; } 

        public virtual ICollection<AudienceType> AudienceTypeTags { get; set; }
        
        
    }
}
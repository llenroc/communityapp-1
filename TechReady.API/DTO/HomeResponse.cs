using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechReady.Common.Models;

namespace TechReady.Common.DTO
{
    public class HomeResponse
    {
        public ObservableCollection<TechReady.Common.Models.Event> AllEvents { get; set; }
    }

    public class FeedbackResponse
    {
        public string ResponseText { get; set; }
    }

    public class FollowEventResponse
    {
    }

    public class FollowSpeakerResponse
    {
    }

    public class WatchedLearningResourceResponse
    {
    }

    public class NotificationsResponse
    {
        public ObservableCollection<Notification> UserNotifications { get; set; } 
    }

    public class MarkNotificationResponse
    {
        
    }
}

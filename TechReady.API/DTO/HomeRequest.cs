using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechReady.Common.DTO
{
    public class LearningResourcesRequest
    {
        public string UserRole { get; set; }

        public List<string> Technologies { get; set; }

        public string SourceType { get; set; }

        public int RequestedPageNo { get; set; }
                   
    }

    public class FeedbackRequest
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string AppUserId { get; set; }

        public string FeedbackType { get; set; }

        public string Feedback { get; set; }
    }

    public class FollowEventRequest
    {
        public string EventId { get; set; }
        public string AppUserId { get; set; }

        public bool Follow { get; set; }
    }

    public class FollowSpeakerRequest
    {
        public string SpeakerId { get; set; }
        public string AppUserId { get; set; }

        public bool Follow { get; set; }
    }

    public class WatchedLearningResourceRequest
    {
        public string WatchedLearningResourceId { get; set; }
        public string AppUserId { get; set; }
        
    }

    public class NotificationsRequest
    {
        public string AppUserId { get; set; }

    }

    public class MarkNotificationRequest
    {
        public string AppUserId { get; set; }

        public string NotificationId { get; set; }

    }
}

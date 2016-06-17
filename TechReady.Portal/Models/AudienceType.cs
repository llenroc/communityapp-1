using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace TechReady.Portal.Models
{
    public class AudienceType
    {
        public int AudienceTypeID { get; set; }
        [DisplayName("Audience Type (Student, Developer, etc.)")]
        public string TypeOfAudience { get; set; }


        [JsonIgnore]
        public virtual ICollection<AudienceOrg> AudienceOrg { get; set; }

        [JsonIgnore]    
        public virtual ICollection<Notification> Notifications { get; set; }

        [JsonIgnore]
        public virtual ICollection<LearningResourceFeed> LearningResourceFeeds { get; set; }

        [JsonIgnore]
        public virtual ICollection<LearningResource> LearningResources { get; set; }

        [JsonIgnore]
        public virtual ICollection<Event> Events { get; set; } 
    }
}
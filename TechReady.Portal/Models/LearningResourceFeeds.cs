using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TechReady.Portal.Models
{
    public class LearningResourceFeed
    {
        public int LearningResourceFeedID { get; set; }

        [DisplayName("Primary Technology")]
        public int? PrimaryTechnologyID { get; set; }
        public virtual PrimaryTechnology PrimaryTechnology { get; set; }

      
        public virtual ICollection<AudienceType> AudienceTypes { get; set; }

        public int? LearningResourceTypeID { get; set; }
        public LearningResourceType LearningResourceType { get; set; }

        [DisplayName("RSS Link")]
        [Required]
        public string RSSLink { get; set; }

    }
}

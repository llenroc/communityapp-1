using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TechReady.Portal.Models
{

    public class LearningResource
    {
     
        public int LearningResourceID { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [DisplayName("Thumbnail URL")]
        public string ThumbnailURL { get; set; }

       [DisplayName("Learning Resource Type")]
        public int? LearningResourceTypeID { get; set; }

        public virtual LearningResourceType LearningResourceType { get; set; }

        [DisplayName("Content URL")]
        [Required]
        public string ContentURL { get; set; }

        public DateTime PublicationTime { get; set; }


        [DisplayName("Primary Technology")]
        public int? PrimaryTechnologyID { get; set; }
        public virtual PrimaryTechnology PrimaryTechnology { get; set; }

        
        public virtual ICollection<AudienceType>  AudienceTypes { get; set; }

        public virtual ICollection<AppUser> DismissedByUsers { get; set; } 

    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TechReady.Portal.Models
{
    public class LearningResourceType
    {
        public int LearningResourceTypeID { get; set; }

        [DisplayName("Learning Resource Type")]
        [Required]
        public string LearningResourceTypeName { get; set; }
    }
}
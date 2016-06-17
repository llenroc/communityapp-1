using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TechReady.Portal.Models
{
    public class Feedback
    {

        public int FeedbackID { get; set; }
        //One feedback will be for one TrackAgenda ENTITY

        public int TrackAgendaID { get; set; }

 
        public virtual TrackAgenda TrackAgenda { get; set; }
        [DisplayName("Name Token")]
        [Required]
        public string NameToken { get; set; }
        [DisplayName("Content Rating")]
        [Required]
        [Range(1,10)]
        public float ContentRating { get; set; }
        [DisplayName("Speaker Rating")]
        [Required]
        [Range(1,10)]
        public float SpeakerRating { get; set; }
        [DisplayName("Overall Rating")]
        [Required]
        [Range(1,10)]
        public float OverallRating { get; set; }
        [DisplayName("LTR Rating")]
        [Required]
        [Range(1,10)]
        public float LTRRating { get; set; }
        [DisplayName("Review")]
        [Required]
        [StringLength(100)]
        public string Review { get; set; }
    }
}
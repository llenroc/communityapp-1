using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TechReady.Portal.Models
{
    public enum Affiliation
    {
        Microsoft, Microsoft_Family, External
    }

    public enum Type
    {
        Individual, Panel
    }
    public class Speaker
    {
        public int SpeakerID { get; set; }
        [DisplayName("Speaker Name")]
        [Required]
        [StringLength(30)]
        public string SpeakerName { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Profile { get; set; }
                
               
        [DataType(DataType.Url)]
        [DisplayName("Picture URL")]
        public string PicUrl { get; set; }
        public Affiliation Affiliation { get; set; }

        public string Title { get; set; }

        [DataType(DataType.EmailAddress)]
        [DisplayName("Email address")]
        [Required]
        [StringLength(30)]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [DisplayName("Phone Number")]
        [Required]
        [StringLength(13)]
        public string MobilePhone { get; set; }

        [DisplayName("Twitter Handle")]
        [Required]
        [StringLength(30)]
        public string TwitterHandle { get; set; }

        public Type Type { get; set; }

        [DisplayName("Recommended by")]
        [Required]
        [StringLength(30)]
        public string RecommendedBy { get; set; }
        public int SpeakerLevel { get; set; }

        //One speaker can be marked to different TrackAgenda entities 
        public virtual ICollection<TrackAgenda> TrackAgendas { get; set; }


        public string BlogLink { get; set; }
        public string FacebookLink { get; set; }
        public string LinkedInLink { get; set; }
        public string Location { get; set; }

        [JsonIgnore]
        public virtual ICollection<AppUser> FollowedByUsers { get; set; } 
    }
}
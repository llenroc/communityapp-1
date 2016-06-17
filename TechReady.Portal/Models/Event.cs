using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using TechReady.Portal.Controllers;

namespace TechReady.Portal.Models
{
    public enum scEligibility
    {
        True, False
    }
    public enum EventStatus
    {
        Tentative, Confirmed, Cancelled, Completed
    }
    public enum EventVisibility
    {
        [DefaultValue("Hidden")] Hidden, Visible 
    }

    public enum EventType
    {
        Firstparty_ProDev, Firstparty_StudentDev, Thirdparty, Community_ProDev, Community_StudentDev,Webinar
    }


    public class Event
    {
        public int EventID { get; set; }
        [DisplayName("Event Name")]
        [Required]
        [StringLength(20)]
        public string EventName { get; set; }

        [DisplayName("About the event")]
        [DataType(DataType.MultilineText)]
        public string EventAbstract { get; set; }


        [DisplayName("Date (from)")]
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        
        public DateTime? EventFromDate { get; set; }

        [DisplayName("Date (to)")]
        
        //[GreaterThan("EventFromDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
         public DateTime? EventToDate { get; set; }

        [DisplayName("Venue")]
        [Required]
        [StringLength(300)]
        public string EventVenue { get; set; }

        [DisplayName("Registration Link")]
        
        [DataType(DataType.Url)]
        public string RegLink { get; set; }
        [DisplayName("[Internal use] Max capacity (Enter a number)")]
        [Required]
        [Range (1,5000)]
        public int MaxCapacity { get; set; }
        [DisplayName("[Internal use] Scorecard Eligibility")]
        [Required]
        public scEligibility ScEligibility { get; set; }
        [DisplayName("[Internal use] Registration Capacity")]
        [Required]
        [Range(1, 5000)]
        public int RegCapacity { get; set; }
        [DisplayName("[Internal use] Publish to WWE")]
        
        public bool PubtoMSCOM { get; set; }
        [DisplayName("[Internal Use] Post Registered")]
        [Range(1, 5000)]
        public int? PostRegistered { get; set; }

        [DisplayName("[Internal Use] Post Attended")]
        [Range(1, 5000)]
        public int? PostAttended { get; set; }
        [DisplayName("[Internal Use] Overall Manual Event Rating")]
       
        [Range(1, 10)]
        public float? PostManualOverallRating { get; set; }
        [DisplayName("[Internal use] AMM Owner")]
        [Required]
        [StringLength(50)]
        public string EventOwner { get; set; }
        [DisplayName("Event Status")]
        [Required]
        public EventStatus EventStatus { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(1000)]
        public string Reason { get; set; }  //This explains why the Event  was cancelled. This is kept only in the model
                                            //and not in the Views for now. On selecting the option for Tentative/Cancelled
                                            //this textbox should come up in the views.


        [DisplayName("Event Visibility")]
        public EventVisibility EventVisibility { get; set; }
        [DisplayName("[Internal Use] Event Type")]
        [Required]
        public EventType EventType { get; set; }
        [JsonIgnore]
        public virtual ICollection<EventTrack> EventTracks { get; set; }    //One event can have multiple EventTracks

        [DisplayName("City Name")]
        public string CityName { get; set; }      //One event entity will be assigned to one city
        [JsonIgnore]
        public virtual City City { get; set; }

        [DisplayName("Global Event")]
        public bool IsGlobal { get; set; }


        public virtual ICollection<PrimaryTechnology> EventTechnologTags { get; set; }

        public virtual ICollection<AudienceType> EventAudienceTypeTags { get; set; }

            
            [JsonIgnore]
        public virtual ICollection<AppUser> FollowedByUsers { get; set; }


        [NotMapped]
        public KeyValueObject EventTypeObj { get; set; }

        [NotMapped]
        public KeyValueObject EventStatusObj { get; set; }

        [NotMapped]
        public KeyValueObject EventVisibilityObj { get; set; }


        [NotMapped]
        public KeyValueObject EventScEligibilityObj { get; set; }

        //public virtual ICollection<City> Citys { get; set; }
    }

}
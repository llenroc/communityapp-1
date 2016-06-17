using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TechReady.Portal.Models
{
    public class EventTrack
    {

        [DisplayName("Event Track ID")]
        [Required]
        
        public int EventTrackID { get; set; }
       
        public int EventID { get; set; }    //Foreign key to Event model
        //[Display(Name = "Event")]
        //[Required]
        [JsonIgnore]
        public virtual Event Event { get; set; }
       
        public int TrackID { get; set; }    //Foreign key to Track model
        //[Display(Name = "Track")]
        //[Required]
        [JsonIgnore]
        public virtual Track Track { get; set; }


        [JsonIgnore]
        //One Event track will have many Track Agendas
        public virtual ICollection<TrackAgenda> TrackAgendas { get; set; }
        [DisplayName("Venue")]
        [Required]
        [StringLength(50)]
        public string TrackVenue { get; set; }

        //[DisplayName("Track Date")]
        //[Required]
        //[DataType(DataType.Date)]
        //public DateTime TrackDate { get; set; }


        [DisplayName("Start Date Time")]
        [Required]
        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:d}")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime? TrackStartTime { get; set; }
       
              
        [DisplayName("End Date Time")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime TrackEndTime { get; set; }
        
        
        
        [DisplayName("Track Seating")]
        [Required]
        [Range(1,5000)]
        public int TrackSeating { get; set; }
        [DisplayName("Track Owner")]
        [Required]
        [StringLength(20)]
        public string TrackOwner { get; set; }

        
    }

}
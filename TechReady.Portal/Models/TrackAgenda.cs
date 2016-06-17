using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TechReady.Portal.Models
{
    public class TrackAgenda
    {
        public int TrackAgendaID { get; set; }

        //A Track Agenda will be mapped to one Event Track
        public int EventTrackID { get; set; }   
        [JsonIgnore]
        public virtual EventTrack EventTrack { get; set; }

        
        public int SpeakerID { get; set; }
        [JsonIgnore]
        public virtual Speaker Speaker { get; set; }


        //One Track Agenda ENTITY will have one Session
        //public virtual ICollection<Session> Session { get; set; }

        public int SessionID { get; set; }
        [JsonIgnore]
        public virtual Session Session { get; set; }
        //[Required]
        //[DisplayName("Event-track-session-speaker")]
        //public string Event_Track_Session_Speakers { get; set; }

        //These are the start and end times of one particular session
       
        [DisplayName("Start time")]
         [Required]
         [DataType(DataType.Time)]
         //[DataType(DataType.DateTime)]
         //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
         public DateTime StartTime { get; set; }
         
        [DisplayName("End time")]
         [Required]
         [DataType(DataType.Time)]
         ////[DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }

         [DisplayName("Favorites count")]
         [DefaultValue(0)]
        public int FavCount { get; set; }
         [DisplayName("QR Code URL")]
        [DataType(DataType.Url)]
        public string QRCode { get; set; }
    }
}
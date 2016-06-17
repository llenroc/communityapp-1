using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TechReady.Portal.Models
{
    public enum Format
    {
        Breakout, HOL, Hackathon
    }

    public class Track
    {
        public int TrackID { get; set; }
        [DisplayName("Track Display Name")]
        [Required]
        [StringLength(50)]
        public string TrackDisplayName { get; set; }
        [DisplayName("Abstract of track")]
        [DataType(DataType.MultilineText)]
        [Required]
       public string TrackAbstract { get; set; }


        
        public int AudienceTypeID { get; set; }
        public virtual AudienceType AudType { get; set; }



        [DisplayName("Internal Track Name")]
        [StringLength(35)]
        public string InternalTrackName { get; set; }

        public Format Format { get; set; }

        public int ThemeID { get; set; }
        public virtual Theme Theme{ get; set; }

        public virtual ICollection<Session> Session { get; set; }

        public virtual ICollection<EventTrack> EventTracks { get; set; }
    }
}

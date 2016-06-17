using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TechReady.Portal.Models
{
    //To DO: One session can only be in one Track Agenda - Rightnow it can be in many
    public class Session
    {
        public int SessionID { get; set; }

        [DisplayName("Session Sequence")]
        [Required]
        [Range(1,30)]
        public int SessionNo { get; set; }
        
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        public string Abstract { get; set; }
        [DisplayName("Level (100,200,300,400)")]
        [Required]
        public int TechLevel { get; set; }



        
        [DisplayName("Primary Technology")]
      
        public int PrimaryTechnologyID { get; set; }
        [JsonIgnore]
        public virtual PrimaryTechnology PrimaryTechnology { get; set; }
        
        [DisplayName("Secondary Technology")]
        //[Required]
        //[StringLength(100)]
        public int SecondaryTechnologyID { get; set; }
        [JsonIgnore]
        public virtual SecondaryTechnology SecondaryTechnology { get; set; }



        public string Product { get; set; }

        [DisplayName("Mention the infrastructure needs")]
        [StringLength(500)]
        public string InfraNeeds { get; set; }

        [DisplayName("Pre-requisites")]
        [StringLength(500)]
        public string PreRequisites { get; set; }

        [DisplayName("Post-requisites")]
        [StringLength(500)]
        public string PostRequisites { get; set; }

        public int TrackID { get; set; }    //One session will be in one Track
        [JsonIgnore]
        public virtual Track Track { get; set; }
        [JsonIgnore]
        public virtual ICollection<TrackAgenda> TrackAgenda { get; set; }



    }
}
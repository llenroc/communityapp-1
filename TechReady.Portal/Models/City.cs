using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using Newtonsoft.Json;

namespace TechReady.Portal.Models
{
    public enum Zone
    { 
    North, South, East, West, Central
    }

    public class City
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int CityID { get; set; }

        [Required]
        [DisplayName("City Name")]
        [Key]
        //[MetadataType(typeof(CityMetaData))]
        //[Remote("DoesCityExist", "Cities", ErrorMessage = "City already exists")]
        [StringLength(40)]
        public string CityName { get; set; }
        [Required]
        [StringLength(40)]
        public string State { get; set; }       
        public Zone Zone { get; set; }

        public DbGeography Location { get; set; }

        [JsonIgnore]
        public virtual ICollection<Event> Event { get; set; }
    }
}

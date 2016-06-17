using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TechReady.Portal.Models
{
    public class PrimaryTechnology
    {
        public int PrimaryTechnologyID { get; set; }
        [DisplayName("Primary Technology")]
        [Required]
        public string PrimaryTech { get; set; }

        [JsonIgnore]
        public virtual ICollection<SecondaryTechnology> SecondaryTechnologies { get; set; }
        [JsonIgnore]
        public virtual ICollection<AppUser> AppUsers { get; set; }
        [JsonIgnore]
        public virtual ICollection<Notification> Notifications { get; set; }
        [JsonIgnore]
        public virtual ICollection<Event> Events { get; set; } 
    }
}
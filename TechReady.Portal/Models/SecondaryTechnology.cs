using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TechReady.Portal.Models
{
    public class SecondaryTechnology
    {
        public int SecondaryTechnologyID { get; set; }

        [DisplayName("Secondary Technology")]
        [Required]
        public string SecondaryTech { get; set; }

        public int PrimaryTechnologyID { get; set; }
        public virtual PrimaryTechnology PrimaryTechnology { get; set; }

       }
}
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TechReady.Portal.Models
{
    //public enum Audience
    //{
    //    Infra, Dev
    //}
    public class Theme
    {
        public int ThemeID { get; set; }
        //One theme can be associated with multiple tracks (Ex: Dev HOL can be in 2 tracks)
        public virtual ICollection<Track> Tracks { get; set; }

        [DisplayName("Theme Name")]
        [Required]
        [StringLength(25)]
        public string ThemeName { get; set; }

        
        //private const int DEFAULT_FY = 16;
        //private int _FY = DEFAULT_FY;
        //[DisplayName("Financial Year")]
        //[Required]
        //public int FY
        //{
        //    get { return _FY; }
        //    set { _FY = value; }
        //}

        [DisplayName("Financial Year")]
        [DefaultValue("FY16")]
        public string FY { get; set; }

        //public Audience Audience { get; set; }
    }
}

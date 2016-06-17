using System.ComponentModel;

namespace TechReady.Portal.Models
{
    public class AudienceOrg
    {
        public int AudienceOrgID { get; set; }

        [DisplayName("Audience Organization")]
        public string AudOrg { get; set; }

        public int AudienceTypeID { get; set; }


        public virtual AudienceType AudienceType1 { get; set; }

    }
}
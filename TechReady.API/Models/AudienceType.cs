using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechReady.Common.Models
{
    public  class AudienceOrg
    {
        public int AudienceOrgId { get; set; }
        public string AudienceOrgName { get; set; }

        public string AudienceTypeName { get; set; }


        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            AudienceOrg c = (AudienceOrg)obj;
            return (AudienceOrgId == c.AudienceOrgId);
        }
        public override int GetHashCode()
        {
            return this.AudienceOrgId.GetHashCode();
        }

        public override string ToString()
        {
            return this.AudienceOrgName;
        }
    }

    public partial class AudienceType
    {
        public string AudienceTypeName { get; set; }
        public int AudienceTypeId { get; set; }


        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            AudienceType c = (AudienceType)obj;
            return (AudienceTypeId == c.AudienceTypeId);
        }
        public override int GetHashCode()
        {
            return this.AudienceTypeId.GetHashCode();
        }


        public override string ToString()
        {
            return this.AudienceTypeName;
        }
    }
}

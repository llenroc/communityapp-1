using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechReady.Common.Models;

namespace TechReady.Common.DTO
{
    public class TechnologiesResponse
    {
        public List<AudienceType> AudienceTypes { get; set; }
            
        public List<AudienceOrg> AudienceOrgTypes { get; set; }

        public List<SecondaryTechnology> SecondaryTechnologies { get; set; }

        public List<City> Cities { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechReady.Common.Models
{
    public class SecondaryTechnology
    {
        public int SecondaryTechnologyId { get; set; }

        public string SecondaryTechnologyName { get; set; }

        public string PrimaryTechnologyName { get; set; }

        public int PrimaryTechnologyId { get; set; }

        public bool IsSelected { get;
            set; }
    }
}

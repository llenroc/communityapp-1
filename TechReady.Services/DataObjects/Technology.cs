using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;
using Newtonsoft.Json;

namespace TechReady.Services.DataObjects
{

    public class TechnologyDTO : EntityData
    {
        public string TechnologyName { get; set; }

    }

    public class Technology
    {

        public Technology()
        {
            this.UserProfiles = new HashSet<UserProfile>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string TechnologyName { get; set; }

        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}

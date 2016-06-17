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

    public class UserProfileDTO : EntityData
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Location { get; set; }

        public string Usertype { get; set; }

        public virtual ICollection<TechnologyDTO> InterestedIn { get; set; }
    }




    public class UserProfile
    {

        public UserProfile()
        {
            this.InterestedIn = new HashSet<Technology>();
        }


        public string Username { get; set; }

        [Key]
        public string Id { get; set; }

        public string Email { get; set; }

        public string Location { get; set; }

        public string Usertype { get; set; }

        public virtual ICollection<Technology> InterestedIn { get; set; }
    
    }



}

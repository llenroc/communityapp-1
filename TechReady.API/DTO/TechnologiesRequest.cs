using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechReady.API.Models;
using TechReady.Common.Models;

namespace TechReady.Common.DTO
{
    public class TechnologiesRequest
    {

    }

    public class ProfileRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string AuthProvider { get; set; }
        public string AuthProviderUserId { get; set; }

        public string DeviceId { get; set; }
        public City City { get; set; }

        public GeoCodeItem Location { get; set; }

        public string Town { get; set; }
        public AudienceType SelectedAudienceType { get; set; }
        public AudienceOrg SelectedAudienceOrgType { get; set; }

        public List<SecondaryTechnology> SecondaryTechnologies { get; set; }
        public string DevicePlatform { get; set; }
        public bool PushEnabled { get; set; }
        public string PushId { get; set; }
    }

    public class ProfileResponse
    {
        public int UserId { get; set; }

        public string DeviceId { get; set; }
    }

    public class CheckProfileRequest
    {
        public string AuthProviderUserId { get; set; }

        public string AuthProvider { get; set; }

    }

    public class CheckProfileResponse : TechnologiesResponse
    {
        public bool IsRegistered { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string AuthProvider { get; set; }
        public string AuthProviderUserId { get; set; }
        public City City { get; set; }

        public string Town { get; set; }
        public AudienceType SelectedAudienceType { get; set; }
        public AudienceOrg SelectedAudienceOrgType { get; set; }

        public GeoCodeItem Location { get; set; }
        public string DeviceId { get; set; }
        public string UserId { get; set; }
    }
}

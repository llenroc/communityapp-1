using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechreadyForms.Helpers.GeoLocationHelper;
using TechReady.API.Models;
using TechReady.Common.Models;
using Xamarin.Forms;

namespace TechReady.Helpers.GeoLocationHelper
{
    public class LocationHelper
    {
        public string FoundCity { get; set; }
        public string FoundTown { get; set; }
        public GeoCodeItem UserLocation { get; set; }
    }    

    class GeoLocationHelper
    {
        public static async Task<LocationHelper> GetCity(List<City> cities)
        {
            var service = DependencyService.Get<IGeoLocationHelper>();
            return await service.GetCity(cities);
        }

    }
}

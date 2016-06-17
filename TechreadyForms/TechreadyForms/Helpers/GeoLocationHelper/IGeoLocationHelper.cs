using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechReady.Common.Models;
using TechReady.Helpers.GeoLocationHelper;

namespace TechreadyForms.Helpers.GeoLocationHelper
{
    public interface IGeoLocationHelper
    {
        Task<LocationHelper> GetCity(List<City> cities);
    }
}

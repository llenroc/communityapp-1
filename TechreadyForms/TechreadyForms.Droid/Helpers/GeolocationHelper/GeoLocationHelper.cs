using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TechreadyForms.Helpers.GeoLocationHelper;
using TechReady.API.Models;
using TechReady.Common.Models;
using TechReady.Helpers.GeoLocationHelper;
using Xamarin.Forms;

[assembly: Xamarin.Forms.DependencyAttribute(typeof(TechreadyForms.Droid.Helpers.GeolocationHelper.GeoLocationHelper))]


namespace TechreadyForms.Droid.Helpers.GeolocationHelper
{
    class GeoLocationHelper : IGeoLocationHelper
    {
        public async Task<LocationHelper> GetCity(List<City> cities)
        {
            var returnVal = new LocationHelper();

            var _locationManager = (LocationManager)Forms.Context.GetSystemService(Activity.LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };

            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            String _locationProvider;
            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = String.Empty;
            }

            var location = _locationManager.GetLastKnownLocation(_locationProvider);


            Dictionary<string, double> cityDistances = new Dictionary<string, double>();

            foreach (var city in cities)
            {
                if (city.Location != null && city.Location.Longitude != null && city.Location.Latitude != null)
                {

                    var distance = DistanceTo(location.Latitude, location.Longitude, city.Location.Latitude.Value,
                        city.Location.Longitude.Value);

                    cityDistances.Add(city.CityName, Math.Abs(distance));
                }
            }

            if (cityDistances.Count > 0)
            {
                var shortest = cityDistances.Aggregate((c, d) => c.Value < d.Value ? c : d);
                returnVal.FoundCity = shortest.Key;
            }
            else
            {
                returnVal.FoundCity = "";
            }


            returnVal.UserLocation = new GeoCodeItem()
            {
                Longitude = location.Longitude,
                Latitude = location.Latitude
            };

            returnVal.FoundTown = "";

            return returnVal;

        }

        public double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            switch (unit)
            {
                case 'K': //Kilometers -> default
                    return dist * 1.609344;
                case 'N': //Nautical Miles 
                    return dist * 0.8684;
                case 'M': //Miles
                    return dist;
            }

            return dist;
        }
    }
}
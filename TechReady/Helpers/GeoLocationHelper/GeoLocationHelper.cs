using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using TechReady.API.Models;
using TechReady.Common.Models;

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
            var returnVal = new LocationHelper();
        

            try
            {
                var geolocator = new Geolocator();
                geolocator.DesiredAccuracyInMeters = 100;
                Geoposition position = await geolocator.GetGeopositionAsync();

                // reverse geocoding
                BasicGeoposition myLocation = new BasicGeoposition
                {
                    Longitude = position.Coordinate.Longitude,
                    Latitude = position.Coordinate.Latitude
                };

                Geopoint pointToReverseGeocode = new Geopoint(myLocation);

                Dictionary<string, double> cityDistances = new Dictionary<string, double>();
                foreach (var city in cities)
                {
                    if (city.Location != null && city.Location.Longitude != null && city.Location.Latitude != null)
                    {

                        var distance = DistanceTo(myLocation.Latitude, myLocation.Longitude, city.Location.Latitude.Value,
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
                    Longitude = myLocation.Longitude,
                    Latitude = myLocation.Latitude
                };

                MapLocationFinderResult result = await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeocode);

                returnVal.FoundTown = result.Locations[0].Address.Town;

            }
            catch (Exception)
            {
                
                
            }
           
            // here also it should be checked if there result isn't null and what to do in such a case
            return returnVal;
        }

        static public double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
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

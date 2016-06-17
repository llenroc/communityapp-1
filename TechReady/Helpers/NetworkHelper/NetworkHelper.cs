using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace TechReady.Helpers.NetworkHelper
{
    public class NetworkHelper
    {
        public static bool IsNetworkAvailable()
        {
            return GetNetworkAvailable();
        }

        public static bool GetNetworkAvailable()
        {
            try
            {
                var profile = NetworkInformation.GetInternetConnectionProfile();
                if (profile == null)
                {
                    return false;

                }
#if DEBUG
                var emulatorConnectivityLevel = profile.GetNetworkConnectivityLevel();  // check for debuging emulators
                if (emulatorConnectivityLevel == NetworkConnectivityLevel.LocalAccess)
                {
                    return true;
                }
#endif
                var connectivityLevel = profile.GetNetworkConnectivityLevel();
                if (connectivityLevel != NetworkConnectivityLevel.InternetAccess)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }



        }

    }
}

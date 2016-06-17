using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechreadyForms.Helpers.NetworkHelper;
using Xamarin.Forms;

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
            INetworkHelper networkHelper = DependencyService.Get<INetworkHelper>();
            return networkHelper.IsNetworkAvailable();
        }

    }
}

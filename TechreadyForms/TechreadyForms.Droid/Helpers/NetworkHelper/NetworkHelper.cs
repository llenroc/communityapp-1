using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Net;
using TechreadyForms.Helpers.NetworkHelper;
using TechReady.Helpers;
using Xamarin.Forms;
using XLabs.Forms.Controls;

[assembly: Xamarin.Forms.DependencyAttribute(typeof(TechreadyForms.Droid.Helpers.NetworkHelper.NetworkHelper))]

namespace TechreadyForms.Droid.Helpers.NetworkHelper
{
    public class NetworkHelper : INetworkHelper
    {
        public bool IsNetworkAvailable()
        {
            ConnectivityManager connectivityManager =   (ConnectivityManager)Forms.Context.GetSystemService(Context.ConnectivityService);
            NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;

            var task = IsReachable();
           
            bool isOnline = (activeConnection != null) && activeConnection.IsConnected;
            return isOnline;
        }

        /// <summary> 
        /// Determines whether the specified host is reachable. 
        /// </summary> 
        /// <param name="host">The host.</param> 
        /// <param name="timeout">The timeout.</param> 
        public Task<bool> IsReachable()
        {
            return Task.Run(
                () =>
                {
                    try
                    {
                        var address = InetAddress.GetByName(CommonSettings.ServiceHost);


                        return address != null; // && (address.IsReachable((int)timeout.TotalMilliseconds) || ); 
                    }
                    catch (UnknownHostException)
                    {
                        return false;
                    }
                });
        }

    }
}
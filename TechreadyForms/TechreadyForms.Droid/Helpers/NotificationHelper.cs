using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Gcm;
using Android.Gms.Gcm.Iid;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Gcm.Client;
using TechreadyForms.Helpers;
using TechReady.Helpers;
using Xamarin.Forms;
using XLabs.Forms.Controls;
using Task = System.Threading.Tasks.Task;

[assembly: Xamarin.Forms.DependencyAttribute(typeof(TechreadyForms.Droid.Helpers.NotificationHelper))]

namespace TechreadyForms.Droid.Helpers
{
    class NotificationHelper : INotificationHelper
    {
       // static object locker = new object();

        public Task<string> GetPushId()
        {
            return Task.Factory.StartNew<string>(() =>
            {
                var token = "";
                try
                {
                    if (IsPlayServicesAvailable())
                    {
                        //lock (locker)
                        //{
                        var instanceID = InstanceID.GetInstance(Forms.Context);
                        token = instanceID.GetToken(
                            CommonSettings.SenderID, GoogleCloudMessaging.InstanceIdScope, null);
                        return token;
                        //}
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception ex)
                {

                }
                return token;

            });
        }


        public bool IsPlayServicesAvailable()
        {
            int resultCode = GooglePlayServicesUtil.IsGooglePlayServicesAvailable(Forms.Context);

            if (resultCode != ConnectionResult.Success)
            {
                
                return false;
            }
            else
            {
                
                return true;
            }
        }

    }
}
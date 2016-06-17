using System;
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Gcm.Client;
using TechReady.Helpers;

namespace TechreadyForms.Droid
{
    [Activity(Exported = true,Label = "India Tech Community", Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {

        public static MainActivity instance;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            RequestedOrientation = ScreenOrientation.Portrait;




            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());

            UserDialogs.Init(this);

            instance = this;
        }

    }
}


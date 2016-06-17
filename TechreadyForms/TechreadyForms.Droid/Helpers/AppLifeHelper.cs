using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TechreadyForms.Helpers;

[assembly: Xamarin.Forms.DependencyAttribute(typeof(TechreadyForms.Droid.Helpers.AppLifeHelper))]

namespace TechreadyForms.Droid.Helpers
{
    class AppLifeHelper : IAppLifeHelper
    {
        public void Kill()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}
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
using TechreadyForms.Helpers.ShareHelper;
using Xamarin.Forms;

[assembly: Xamarin.Forms.DependencyAttribute(typeof(TechreadyForms.Droid.Helpers.ShareHelper.ShareHelper))]

namespace TechreadyForms.Droid.Helpers.ShareHelper
{
    class ShareHelper : IShareHelper
    {
        public void ShareData(string textToShare)
        {
            Intent sendIntent = new Intent(Android.Content.Intent.ActionSend);
            sendIntent.PutExtra(Android.Content.Intent.ExtraText, textToShare);
            sendIntent.SetType("text/plain");
            Forms.Context.StartActivity(sendIntent);
        }
    }
}
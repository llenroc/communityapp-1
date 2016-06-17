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
using TechreadyForms.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(RootNavigationRenderer))]
namespace TechreadyForms.Droid.CustomRenderer
{
    public class RootNavigationRenderer : NavigationRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<NavigationPage> e)
        {
            base.OnElementChanged(e);

            RemoveAppIconFromActionBar();
        }

        void RemoveAppIconFromActionBar()
        {
            // http://stackoverflow.com/questions/14606294/remove-icon-logo-from-action-bar-on-android
            var actionBar = ((Activity)Context).ActionBar;
            actionBar.SetIcon(Resource.Color.transparent);
            
        }
    }
}
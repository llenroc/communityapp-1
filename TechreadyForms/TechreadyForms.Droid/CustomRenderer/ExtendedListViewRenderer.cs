using System;
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
using TechreadyForms.Helpers;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Color = Android.Graphics.Color;
using ListView = Xamarin.Forms.ListView;

[assembly: ExportRendererAttribute(typeof(ExtendedListView), typeof(ExtendedListViewRenderer))]

namespace TechreadyForms.Droid.CustomRenderer
{

    class ExtendedListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            Control.SetSelector(Android.Resource.Color.Transparent);

        }
    }
}
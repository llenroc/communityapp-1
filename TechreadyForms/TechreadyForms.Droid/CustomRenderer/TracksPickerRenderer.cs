using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TechreadyForms.Droid.CustomRenderer;
using TechreadyForms.Helpers;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Color = Android.Graphics.Color;

[assembly: ExportRendererAttribute(typeof(TracksPicker), typeof(TracksPickerRenderer))]

namespace TechreadyForms.Droid.CustomRenderer
{

    public class TracksPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetTextColor(Color.White);
              
            }
        }
    }
}
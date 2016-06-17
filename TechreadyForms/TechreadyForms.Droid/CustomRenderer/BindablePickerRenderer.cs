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

[assembly: ExportRendererAttribute(typeof(BindablePicker), typeof(BindablePickerRenderer))]

namespace TechreadyForms.Droid.CustomRenderer
{

    class BindablePickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetTextColor(Color.Black);
            }
        }
    }
}
using System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using TechreadyForms.Droid.CustomRenderer;
using TechreadyForms.Helpers;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Color = Android.Graphics.Color;

[assembly: ExportRendererAttribute(typeof(CustomLabel), typeof(CustomLabelRenderer))]

namespace TechreadyForms.Droid.CustomRenderer
{

    class CustomLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var button = (CustomLabel) e.NewElement;
                Control.SetMaxLines(button.MaxLines);

                Control.Ellipsize = TextUtils.TruncateAt.End;
            }
        }


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "MaxLines")
            {
                if(Control!=null)
                {
                    Control.SetMaxLines(((CustomLabel)sender).MaxLines);       
                }
            }
        }
    }
}
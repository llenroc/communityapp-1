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

[assembly: ExportRendererAttribute(typeof(SearchBar), typeof(CustomSearchBarRenderer))]

namespace TechreadyForms.Droid.CustomRenderer
{

    class CustomSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            SearchView searchView = (base.Control as SearchView);
            searchView.SetInputType(InputTypes.ClassText | InputTypes.TextVariationNormal);

            // Access search textview within control
            int textViewId = searchView.Context.Resources.GetIdentifier("android:id/search_src_text", null, null);
            EditText textView = (searchView.FindViewById(textViewId) as EditText);

            // Set custom colors
          
            textView.SetTextColor(Color.Black);
            
        }
    }
}
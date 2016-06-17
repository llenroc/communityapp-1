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
using Android.Text;

[assembly: Xamarin.Forms.Dependency(typeof(TechreadyForms.Droid.Helpers.TextHelper))]

namespace TechreadyForms.Droid.Helpers
{
    class TextHelper : ITextHelper
    {
        public string ConvertToText(string html)
        {
            return Html.FromHtml(html).ToString();
        }
    }
}
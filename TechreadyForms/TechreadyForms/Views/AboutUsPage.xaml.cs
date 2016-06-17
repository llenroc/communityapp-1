using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace TechreadyForms.Views
{
    public partial class AboutUsPage : ContentPage
    {
        public AboutUsPage()
        {
            InitializeComponent();
        }


        void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {

            var ActionLink = "http://www.microsoft.com/privacystatement/en-in/MicrosoftIndiaWebsites/Default.aspx";

            Navigation.PushAsync(new WebViewPage(ActionLink));

        }
    }
}

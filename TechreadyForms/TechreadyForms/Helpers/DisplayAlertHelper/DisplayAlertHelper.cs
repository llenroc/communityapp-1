using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechreadyForms.Views;
using Xamarin.Forms;

namespace TechreadyForms.Helpers.DisplayAlertHelper
{
    public class DisplayAlertHelper
    {
        static ContentPage page = new ContentPage();

        public static async Task<bool> DisplayMessage(string header, string message, string privacyButton , string okButton)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                var x = await AlertBoxIOSHelper.AlertBoxIOS.ShowDisplayAlert(header, message, privacyButton, okButton);
                return x;
            }
            else
            {
                var okPressed =  await page.DisplayAlert(header, message, privacyButton, okButton);
                return !okPressed;
            }
        }
    }
}

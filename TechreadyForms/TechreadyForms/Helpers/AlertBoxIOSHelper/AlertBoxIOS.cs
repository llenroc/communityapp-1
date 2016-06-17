using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TechreadyForms.Helpers.AlertBoxIOSHelper
{
    public class AlertBoxIOS
    {
        public static async Task<bool> ShowDisplayAlert(string header, string message, string privacyButton , string okButton)
        {
          var x = await DependencyService.Get<IAlertBoxIOS>().ShowDisplayAlert(header, message, privacyButton , okButton);
          return x;
        }
    }
}

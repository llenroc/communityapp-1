using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechreadyForms.Helpers.AlertBoxIOSHelper
{
    public interface IAlertBoxIOS
    {
        Task<bool> ShowDisplayAlert(string header, string message, string privacyButton , string okButton);
    }
}

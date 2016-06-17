using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechreadyForms.Helpers;
using Xamarin.Forms;

namespace TechReady.Helpers
{
    public class NotificationsHelper
    {
        public static string Platform = "gcm";

        public static async Task<string> GetPushId()
        {
            var service = DependencyService.Get<INotificationHelper>();
            return await service.GetPushId();
        }
    }
}

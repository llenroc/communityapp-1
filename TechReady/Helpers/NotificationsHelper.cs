using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechReady.Helpers
{
    public class NotificationsHelper
    {
        public static string Platform = "wns";
        public static async Task<string> GetPushId()
        {
            var channel = await Windows.Networking.PushNotifications.PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

            return channel.Uri;
        }
    }
}

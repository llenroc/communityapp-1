using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;
using TechReady.Portal.Models;
using Notification = TechReady.Portal.Models.Notification;

namespace TechReady.Portal.Helpers
{
    public class NotificationsHubHelper
    {
        private static NotificationHubClient hub;

        public static NotificationHubClient Hub
        {
            get { return hub; }
        }

        static NotificationsHubHelper()
        {
            hub = //change Notifications hub ConnectionString and Hub Name before deploying this
                NotificationHubClient.CreateClientFromConnectionString(
                    "Endpoint=sb://indiatechcommunityhub-ns.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=IBOObWNTr8ZfIKA5YFiE25mKxZImBfMVpxGEFCTU=",
                    "indiatechcommunitymshub");
        }

        public static async Task SendNotificationAsync(List<string> tags, Notification not)
        {
            try
            {


                // Define a Windows Store toast.
                var wnsToast = "<toast><visual><binding template=\"ToastText01\">"
                               + "<text id=\"1\">" + not.NotificationTitle
                               + "</text><text id=\"2\">" + not.NotificationMessage
                               + "</text></binding></visual></toast>";

                await hub.SendWindowsNativeNotificationAsync(wnsToast, tags);

                //// Define a Windows Phone toast.
                //var mpnsToast =
                //    "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                //    "<wp:Notification xmlns:wp=\"WPNotification\">" +
                //        "<wp:Toast>" +
                //            "<wp:Text1>Breaking " + tagExpression + " News!</wp:Text1>" +
                //        "</wp:Toast> " +
                //    "</wp:Notification>";

                //await hub.SendMpnsNativeNotificationAsync(mpnsToast, tagExpression);

                // Define an iOS alert.
                var alert = "{\"aps\":{\"alert\":\"" + not.NotificationTitle + "\"}}";

                await hub.SendAppleNativeNotificationAsync(alert, tags);

                // Define an Android notification.
                var notification = "{\"data\":{\"message\":\"" + not.NotificationTitle + "\"}}";

                await hub.SendGcmNativeNotificationAsync(notification, tags);
            }

            catch (ArgumentException ex)
            {
                // An exception is raised when the notification hub hasn't been 
                // registered for the iOS, Windows Store, or Windows Phone platform. 
            }
        }



        public static async Task SendEventStartNotification(List<Event> events)
        {
            //try
            //{
            //    string Title = "An event you have subscribed to is about to start in 1 Day";



            //    // Define a Windows Store toast.
            //    var wnsToast = "<toast><visual><binding template=\"ToastText01\">"
            //                   + "<text id=\"1\">" + Title
            //                   + "</text></binding></visual></toast>";

            //    await hub.SendWindowsNativeNotificationAsync(wnsToast, tags);

            //    //// Define a Windows Phone toast.
            //    //var mpnsToast =
            //    //    "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            //    //    "<wp:Notification xmlns:wp=\"WPNotification\">" +
            //    //        "<wp:Toast>" +
            //    //            "<wp:Text1>Breaking " + tagExpression + " News!</wp:Text1>" +
            //    //        "</wp:Toast> " +
            //    //    "</wp:Notification>";

            //    //await hub.SendMpnsNativeNotificationAsync(mpnsToast, tagExpression);

            //    // Define an iOS alert.
            //    var alert = "{\"aps\":{\"alert\":\"" + not.NotificationTitle + "\"}}";

            //    await hub.SendAppleNativeNotificationAsync(alert, tags);

            //    // Define an Android notification.
            //    var notification = "{\"data\":{\"msg\":\"" + not.NotificationTitle + "\"}}";

            //    await hub.SendGcmNativeNotificationAsync(notification, tags);
            //}

            //catch (ArgumentException ex)
            //{
            //    // An exception is raised when the notification hub hasn't been 
            //    // registered for the iOS, Windows Store, or Windows Phone platform. 
            //}
        }
    }

}


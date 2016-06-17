using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;

// http://go.microsoft.com/fwlink/?LinkId=290986&clcid=0x409

namespace TechReady
{
    internal class TechReadyServicesPush
    {
        public async static void UploadChannel(List<string> tags)
        {
            var channel = await Windows.Networking.PushNotifications.PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

            try
            {
                await App.TechReadyServicesClient.GetPush().RegisterNativeAsync(channel.Uri,tags);
            }
            catch (Exception exception)
            {
                HandleRegisterException(exception);
            }
        }

        public async static void RemoveChannel(List<string> tags)
        {
            var channel = await Windows.Networking.PushNotifications.PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

            try
            {
                await App.TechReadyServicesClient.GetPush().UnregisterNativeAsync();
            }
            catch (Exception exception)
            {
                HandleRegisterException(exception);
            }
        }

        private static void HandleRegisterException(Exception exception)
        {

        }
    }
}

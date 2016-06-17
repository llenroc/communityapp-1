using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechReady.Helpers
{
    public class CommonSettings
    {
#if RELEASE_LIVE
        private static string _serviceHost = "api url - production"; //change the api url
#else
        private static string _serviceHost = "api url - test";
#endif


        public const string SenderID = "101589002272"; // Google API Project Number. Change this accordingly
        //change notifications hub connection String
        public const string ListenConnectionString = "Endpoint=sb://indiatechcommunityhub-ns.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=d01JuT4a+cKGC/XhxdjssrHh0WrR3BFXifAfJcujg=";
        //change notifications hub name
        public const string NotificationHubName = "indiatechcommunitymshub";

        public static string ServicePath
        {
            get { return string.Format("https://{0}/", _serviceHost) + "{0}"; }
        }

        public static string ServiceHost
        {
            get { return _serviceHost; }
        }
        public static bool FromRegisterationPage{ get; set; } = false;

        public static string LoginNoNetworkMessage =
            "You require an active internet connection to login. Please check your network and try again";

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechReady.NavigationParameters
{
    public class UserRegistrationPageNavigationParameter : NavigationParameter
    {
        public UserRegistrationPageNavigationParameter(string sourcePageName)
        {
            this.FromPageName = sourcePageName;
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public string AuthProvider { get; set; }
        public string AuthProviderUserId { get; set; }
    }
}

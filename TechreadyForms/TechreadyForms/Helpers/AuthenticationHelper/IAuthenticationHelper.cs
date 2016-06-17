using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using TechReady.NavigationParameters;

namespace TechreadyForms.Helpers.AuthenticationHelper
{
    public interface IAuthenticationHelper
    {
        Task<UserRegistrationPageNavigationParameter> Authenticate(MobileServiceAuthenticationProvider provider);
    }
}

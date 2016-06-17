using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechreadyForms.Helpers
{
   public interface INotificationHelper
    {
        Task<string> GetPushId();
    }
}

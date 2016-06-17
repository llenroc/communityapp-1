using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TechreadyForms.Helpers.ShareHelper
{
    public interface IShareHelper
    {
       void ShareData(string textToShare);

    }

    public class ShareHelper
    {
        public static void Share(string text)
        {
            var service = DependencyService.Get<IShareHelper>();
            service.ShareData(text);
        }
    }
}

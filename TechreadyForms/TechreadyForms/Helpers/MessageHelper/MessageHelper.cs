using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;

namespace TechReady.Helpers.MessageHelper
{
    public class MessageHelper
    {
        public static async Task<bool> ShowMessage(string information)
        {
            UserDialogs.Instance.Alert(information);
            return true;
        }

        public static async Task<bool> ShowMessage(string information, string title)
        {
            throw new NotImplementedException();
            //try
            //{
            //    MessageContentDialog msgDialog = new MessageContentDialog(information, title);
            //    return (await msgDialog.ShowMessage());
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex.Message);
            //    return false;
            //}
        }

        public static async Task<bool> ShowMessage(string information, string title, string firstButtonText, string secondButtonText, Action onPrimaryClick)
        {
            var result = await UserDialogs.Instance.ConfirmAsync(information,title,firstButtonText,secondButtonText);
            if (result)
            {
                onPrimaryClick();
            }
            return result;
        }
    }
}

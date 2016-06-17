using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechReady.Views;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace TechReady.Helpers.MessageHelper
{
    public class MessageHelper
    {
        public static async Task<bool> ShowMessage(string information)
        {
            try
            {
                MessageContentDialog msgDialog = new MessageContentDialog(information);
                return (await msgDialog.ShowMessage());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
        public static async Task<bool> ShowMessage(string information, string title)
        {
            try
            {
                MessageContentDialog msgDialog = new MessageContentDialog(information, title);
                return (await msgDialog.ShowMessage());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public static async Task<bool> ShowMessage(string information, string title, string firstButtonText, string secondButtonText, TypedEventHandler<ContentDialog, ContentDialogButtonClickEventArgs> onPrimaryClick)
        {
            try
            {
                MessageContentDialog msgDialog = new MessageContentDialog(information, title);
                msgDialog.PrimaryButtonText = firstButtonText;
                msgDialog.SecondaryButtonText = secondButtonText;
                msgDialog.PrimaryButtonClick += onPrimaryClick;
                return( await msgDialog.ShowMessage() );
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}

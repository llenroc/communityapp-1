using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TechReady.Views
{
    public sealed partial class MessageContentDialog : ContentDialog
    {
        //public MessageContentDialog()
        //{
        //    this.InitializeComponent();
        //}

        public MessageContentDialog(string content)
        {
            this.InitializeComponent();
            this.Content.Text = content;
        }

        public MessageContentDialog(string content, string title)
        {
            this.InitializeComponent();
            this.Content.Text = content;
            this.Title = title;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
        }

        public async Task<bool> ShowMessage()
        {
            try
            {
                await this.ShowAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        //public async Task<bool> ShowMessage(string content , string title)
        //{
        //    try
        //    {
        //        this.Content.Text = content;
        //        this.Title = title;
        //        await this.ShowAsync();
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }
        //}
    }
}

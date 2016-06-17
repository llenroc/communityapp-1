using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TechReady.Helpers.Storage;
using TechReady.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TechReady.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PrivacyPolicyPage : Page
    {
        public PrivacyPolicyPage()
        {
            this.InitializeComponent();
        }

        private void privacyPolicy_tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof (EventRegistrationPage),
                @"http://click.email.microsoftemail.com/?qs=8bfcd8fe3a12429fc885e5797e705f5a9517bb72609becce06ddc022565e635483668eba2b93ec7c");
        }
    }
}

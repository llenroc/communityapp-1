using TechReady.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using TechReady.Common.Models;
using TechReady.ViewModels;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TechReady.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SessionDetailsPage : Page
    {
       
        public SessionDetailsPage()
        {
            this.InitializeComponent();
            this.DataContext = new SessionDetailsPageViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try {
                if (e.Parameter != null)
                {
                    var parameter = JsonConvert.DeserializeObject<TrackSession>(e.Parameter.ToString() as string);
                    ((SessionDetailsPageViewModel)this.DataContext).Session = parameter;
                }
            }
            catch { }
        }

        private void Speaker_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var grid = sender as Grid;
            if (grid == null)
            {
                return;
            }
            var speaker = grid.Tag as TrackSpeaker;
            if (speaker == null)
            {
                return;
            }
            Frame.Navigate(typeof(SpeakerDetailsPage), JsonConvert.SerializeObject(speaker));
        }
    }
}

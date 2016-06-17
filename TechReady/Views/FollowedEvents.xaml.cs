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
using Newtonsoft.Json;
using TechReady.Common.Models;
using TechReady.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TechReady.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FollowedEvents : Page
    {
        public FollowedEvents()
        {
            this.InitializeComponent();
            this.DataContext = new FollowedEventsViewModel();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.ErrorTextBlock.Visibility = Visibility.Collapsed;
            await((FollowedEventsViewModel) this.DataContext).GetFollowedEvents();
            if(((FollowedEventsViewModel)this.DataContext).AllEvents == null || ((FollowedEventsViewModel)this.DataContext).AllEvents.Count == 0)
            {
                this.ErrorTextBlock.Visibility = Visibility.Visible;
                this.ErrorTextBlock.Text = "Looks like you have not followed any Event";
            }

        }

        private void Event_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var grid = sender as Grid;
            if (grid == null)
            {
                return;
            }
            var ev =
            grid.Tag as Event;

            if (ev == null)
            {
                return;
            }

            Frame.Navigate(typeof (EventDetailsPage), JsonConvert.SerializeObject(ev));

        }

    }
}

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
using TechReady.NavigationParameters;
using TechReady.ViewModels;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TechReady.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FilterEventsPage : Page
    {
        public FilterEventsPage()
        {
            this.InitializeComponent();

            this.DataContext = new EventsFilterPageViewModel();
           
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ((EventsFilterPageViewModel) this.DataContext).GetEvents();

            if (e.Parameter != null)
            {
                var np = JsonConvert.DeserializeObject<EventsFilterPageNavigationParameters>(e.Parameter.ToString());
                ((EventsFilterPageViewModel) this.DataContext).SelectedLocation = np.Location;
                ((EventsFilterPageViewModel) this.DataContext).SelectedRole = np.Role;
                ((EventsFilterPageViewModel) this.DataContext).SelectedTechnology = np.Technology;
            }
        }

        private void Accept_Filter(object sender, RoutedEventArgs e)
        {
                Frame.Navigate(typeof (HubPage),
                    JsonConvert.SerializeObject(new EventsFilterPageNavigationParameters("FilterEventsPage")
                    {
                        Role = ((EventsFilterPageViewModel) this.DataContext).SelectedRole,
                        Location = ((EventsFilterPageViewModel) this.DataContext).SelectedLocation,
                        Technology = ((EventsFilterPageViewModel) this.DataContext).SelectedTechnology
                    }));

                Frame.BackStack.RemoveAt(Frame.BackStack.Count-1);
        }
    }
}

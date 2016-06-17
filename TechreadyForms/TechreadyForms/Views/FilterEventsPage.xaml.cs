using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechReady.NavigationParameters;
using TechReady.ViewModels;
using Xamarin.Forms;

namespace TechreadyForms.Views
{
    public partial class FilterEventsPage : ContentPage
    {
        private EventsFilterPageNavigationParameters _parameter;

        public FilterEventsPage(EventsFilterPageNavigationParameters parameter)
        {
            _parameter = parameter;
            InitializeComponent();

            this.BindingContext = new EventsFilterPageViewModel();

            this.NavigatedTo();
        }

        private async void NavigatedTo()
        {
            await ((EventsFilterPageViewModel)this.BindingContext).GetEvents();

            if (_parameter != null)
            {
                var np = _parameter;
                ((EventsFilterPageViewModel)this.BindingContext).SelectedLocation = np.Location;
                ((EventsFilterPageViewModel)this.BindingContext).SelectedRole = np.Role;
                ((EventsFilterPageViewModel)this.BindingContext).SelectedTechnology = np.Technology;
            }
        }

        private async void Accept_click(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(
                new HubPage(
                    JsonConvert.SerializeObject(new EventsFilterPageNavigationParameters("FilterEventsPage")
                    {
                        Role = ((EventsFilterPageViewModel) this.BindingContext).SelectedRole,
                        Location = ((EventsFilterPageViewModel) this.BindingContext).SelectedLocation,
                        Technology = ((EventsFilterPageViewModel) this.BindingContext).SelectedTechnology
                    })));

        }
    }
}

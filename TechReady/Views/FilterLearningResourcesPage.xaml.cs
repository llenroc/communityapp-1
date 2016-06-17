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
    public sealed partial class FilterLearningResourcesPage : Page
    {
        public FilterLearningResourcesPage()
        {
            this.InitializeComponent();
            this.DataContext = new FilterLearningResourcesPageViewModel();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await ((FilterLearningResourcesPageViewModel) this.DataContext).GetLearningResources();

            if (e.Parameter != null)
            {
                var np = JsonConvert.DeserializeObject<LearningResourcesFilterPageNavigationParameters>(e.Parameter.ToString());
                ((FilterLearningResourcesPageViewModel)this.DataContext).SelectedType = np.Type;
                ((FilterLearningResourcesPageViewModel)this.DataContext).SelectedRole = np.Role;
                ((FilterLearningResourcesPageViewModel)this.DataContext).SelectedTechnology = np.Technology;
            }
        }

        private void AcceptFilter_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HubPage),
             JsonConvert.SerializeObject(new LearningResourcesFilterPageNavigationParameters("FilterLearningResourcesPage")
             {
                 Role = ((FilterLearningResourcesPageViewModel)this.DataContext).SelectedRole,
                 Type = ((FilterLearningResourcesPageViewModel)this.DataContext).SelectedType,
                 Technology = ((FilterLearningResourcesPageViewModel)this.DataContext).SelectedTechnology
             }));

            Frame.BackStack.RemoveAt(Frame.BackStack.Count - 1);
        }
    }
}

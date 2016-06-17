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
    public partial class LearningResourcesFilteredPage : ContentPage
    {
        private LearningResourcesFilterPageNavigationParameters _parameter;
        public LearningResourcesFilteredPage(LearningResourcesFilterPageNavigationParameters parameter)
        {
            this._parameter = parameter;
            this.Appearing += LearningResourcesFilteredPage_Appearing; 
            InitializeComponent();

            this.BindingContext = new FilterLearningResourcesPageViewModel();
          
        }

        private void LearningResourcesFilteredPage_Appearing(object sender, EventArgs e)
        {
            this.NavigatedTo();
        }

        private async void NavigatedTo()
        {
            if (this.BindingContext != null)
                await ((FilterLearningResourcesPageViewModel)this.BindingContext).GetLearningResources();

            if (_parameter != null)
            {
                var np = _parameter;
                ((FilterLearningResourcesPageViewModel)this.BindingContext).SelectedType = np.Type;
                ((FilterLearningResourcesPageViewModel)this.BindingContext).SelectedRole = np.Role;
                ((FilterLearningResourcesPageViewModel)this.BindingContext).SelectedTechnology = np.Technology;
            }
        }
        private void AcceptFilter_Click(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(
                new HubPage(
                    JsonConvert.SerializeObject(new LearningResourcesFilterPageNavigationParameters("FilterLearningResourcesPage")
                    {
                        Role = ((FilterLearningResourcesPageViewModel)this.BindingContext).SelectedRole,
                        Type = ((FilterLearningResourcesPageViewModel)this.BindingContext).SelectedType,
                        Technology = ((FilterLearningResourcesPageViewModel)this.BindingContext).SelectedTechnology
                    })));
        }
    }
}

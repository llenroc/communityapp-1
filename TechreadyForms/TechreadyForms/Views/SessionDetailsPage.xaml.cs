using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechReady.Common.Models;
using TechReady.ViewModels;
using Xamarin.Forms;

namespace TechreadyForms.Views
{
    public partial class SessionDetailsPage : ContentPage
    {
        private TrackSession _parameter;
        public SessionDetailsPage(TrackSession parameter)
        {
            this._parameter = parameter;
            this.Appearing += SessionDetailsPage_Appearing;
            this.BindingContext = new SessionDetailsPageViewModel();
            InitializeComponent();
       
        }

        private void SessionDetailsPage_Appearing(object sender, EventArgs e)
        {
            ((SessionDetailsPageViewModel)this.BindingContext).Session = _parameter;
        }

        private void Speaker_Tapped(object sender, EventArgs e)
        {
            if (this.BindingContext != null && ((SessionDetailsPageViewModel)this.BindingContext) != null && ((SessionDetailsPageViewModel)this.BindingContext).Session != null && ((SessionDetailsPageViewModel)this.BindingContext).Session.Speaker != null )
            {
                Navigation.PushAsync(new SpeakerDetailsPage(((SessionDetailsPageViewModel)this.BindingContext).Session.Speaker));
            }
        }
    }
}

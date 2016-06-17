using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechReady.Helpers.MessageHelper;
using TechReady.Helpers.Storage;
using TechReady.ViewModels;
using Xamarin.Forms;

namespace TechreadyForms.Views
{
    public partial class FeedbackPage : ContentPage
    {
        public FeedbackPage()
        {
            InitializeComponent();
            this.BindingContext = new FeedbackPageViewModel();
        }

        private FeedbackPageViewModel ViewModel
        {
            get { return this.BindingContext as FeedbackPageViewModel; }
        }

        private async void SubmitFeedback(object sender, EventArgs e)
        {

            //await ViewModel.SaveFeedback();
            if(await ViewModel.SaveFeedback())
            {

            }

        }

        private void Feedback_OnAppearing(object sender, EventArgs e)
        {
            this.NavigatedTo();
        }

        private async void NavigatedTo()
        {
            var context = await LocalStorage.ReadJsonFromFile<UserRegistrationPageViewModel>("registration");
            ViewModel.UserName = context.FullName;
            ViewModel.Email = context.Email;
        }

        public void OnTextChanged(object sender, TextChangedEventArgs args)
        {
            int maxLength = 100;
            if(args.NewTextValue.Length> maxLength) // to limit the entry of the input field
            {
                (sender as Editor).Text = args.OldTextValue;
            }
        }
    }
}
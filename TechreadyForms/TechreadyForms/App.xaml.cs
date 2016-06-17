using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechreadyForms.Views;
using TechReady.Helpers.Storage;
using TechReady.ViewModels;
using Xamarin.Forms;

namespace TechreadyForms
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var task = LocalStorage.ReadJsonFromFile<UserRegistrationPageViewModel>("registration");

            task.Wait();

            var model = task.Result;

            if (model == null)
            {
                MainPage = new NavigationPage(new LoginPage())
                {
                    BarBackgroundColor = Color.FromHex("#0072c6"),
                    BarTextColor = Color.White
                };
            }
            else
            {
                model.SaveUserRegitration();
                MainPage = new NavigationPage(new HubPage(null))
                {

                };
            }
        }


        protected override async void OnStart()
        {

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

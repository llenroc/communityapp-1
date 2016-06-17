using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TechReady.Common.Models;
using TechReady.Helpers;
using TechReady.Helpers.Storage;

namespace TechReady.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        public SettingsPageViewModel()
        {
        }

        private bool _notificationEnabled;

        public bool NotificationsEnabled
        {
            get
            {
                return _notificationEnabled;
            }
            set
            {
                if (_notificationEnabled != value)
                {
                    _notificationEnabled = value;
                    OnPropertyChanged("NotificationsEnabled");
                }
            }
        }

        public async void SaveSettings()
        {

            var model = await LocalStorage.ReadJsonFromFile<UserRegistrationPageViewModel>("registration");
            if (model != null)
            {
                model.PushEnabled = this.NotificationsEnabled;
                model.SaveUserRegitration();
            }
        }

        public async void  LoadSettings()
        {
            var model = await LocalStorage.ReadJsonFromFile<UserRegistrationPageViewModel>("registration");
            NotificationsEnabled = model.PushEnabled;
        }

    }
}
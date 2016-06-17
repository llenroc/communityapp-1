using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TechReady.API.Models;
using TechReady.Common.DTO;
using TechReady.Common.Models;
using TechReady.Helpers;
using TechReady.Helpers.GeoLocationHelper;
using TechReady.Helpers.MessageHelper;
using TechReady.Helpers.NetworkHelper;
using TechReady.Helpers.ServiceCallers;
using TechReady.Helpers.Storage;
using System.Text.RegularExpressions;

namespace TechReady.ViewModels
{
    public class UserRegistrationPageViewModel : BaseViewModel
    {

        public bool ShowSeperateLocation
        {
            get
            {
                if (string.IsNullOrEmpty(this.Town))
                {
                    return false;
                }
                if (this.City != null && this.Town != this.City.CityName)
                {
                    return true;
                }

                if (this.City == null && string.IsNullOrEmpty(Town) == false)
                {
                    return true;
                }
                return false;
            }
        }

        private string _town;

        public string Town
        {
            get { return _town; }
            set
            {
                if (_town != value)
                {
                    _town = value;
                    OnPropertyChanged("Town");
                }
            }
        }



        private bool _pushEnabled = true;

        public bool PushEnabled
        {
            get { return _pushEnabled; }
            set
            {
                if (_pushEnabled != value)
                {
                    _pushEnabled = value;
                    OnPropertyChanged("PushEnabled");
                }
            }
        }

        private string _deviceId;

        public string DeviceId
        {
            get { return _deviceId; }
            set
            {
                if (_deviceId != value)
                {
                    _deviceId = value;
                    OnPropertyChanged("DeviceId");
                }
            }
        }


        private string _userId;

        public string UserId
        {
            get { return _userId; }
            set
            {
                if (_userId != value)
                {
                    _userId = value;
                    OnPropertyChanged("UserId");
                }
            }
        }




        public GeoCodeItem Location { get; set; }

        private string _fullName;

        public string FullName
        {
            get { return _fullName; }
            set
            {
                if (_fullName != value)
                {
                    _fullName = value;
                    OnPropertyChanged("FullName");
                }
            }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        private string _authProvider;

        public string AuthProvider
        {
            get { return _authProvider; }
            set
            {
                if (_authProvider != value)
                {
                    _authProvider = value;
                    OnPropertyChanged("AuthProvider");
                }
            }
        }

        private string _authProviderUserId;

        public string AuthProviderUserId
        {
            get { return _authProviderUserId; }
            set
            {
                if (_authProviderUserId != value)
                {
                    _authProviderUserId = value;
                    OnPropertyChanged("AuthProviderUserId");
                }
            }
        }

        private City _city;

        public City City
        {
            get { return _city; }
            set
            {
                if (_city != value)
                {
                  
                    _city = value;
                    OnPropertyChanged("City");
                    OnPropertyChanged("CityName");
                    
                }
            }
        }


        public string CityName
        {
            get
            {
                if (this._city != null)
                {
                    return this._city.CityName;
                }
                return "";

            }
            set
            {
                if (Cities == null)
                {
                    return;
                }
                var city = this.Cities.FirstOrDefault(x => x.CityName == value);

                this._city = city;
                OnPropertyChanged("City");
                OnPropertyChanged("CityName");
            }
        }



        private AudienceType _selectedAudienceType;

        public AudienceType SelectedAudienceType
        {
            get { return _selectedAudienceType; }
            set
            {
                if (_selectedAudienceType != value)
                {
                    _selectedAudienceType = value;
                    if (this.AllAudienceOrgTypes != null)
                    {
                        this.AudienceOrgTypes = (from c in this.AllAudienceOrgTypes
                                                 where c.AudienceTypeName == value.AudienceTypeName
                                                 select c).ToList();
                    }
                    OnPropertyChanged("SelectedAudienceType");
                    OnPropertyChanged("AudienceOrgTypes");
                }
            }
        }

        public string SelectedAudienceTypeName
        {
            get
            {
                if (this._selectedAudienceType != null)
                {
                    return this._selectedAudienceType.AudienceTypeName;
                }
                return "";

            }
            set
            {
                if (AudienceTypes == null)
                {
                    return;
                }
                var aut = this.AudienceTypes.FirstOrDefault(x => x.AudienceTypeName == value);

                this._selectedAudienceType = aut;
                if (this.AllAudienceOrgTypes != null)
                {
                    this.AudienceOrgTypes = (from c in this.AllAudienceOrgTypes
                                             where c.AudienceTypeName == aut.AudienceTypeName
                                             select c).ToList();
                }
                OnPropertyChanged("SelectedAudienceType");
                OnPropertyChanged("SelectedAudienceTypeName");
                OnPropertyChanged("AudienceOrgTypes");
            }
        }

        private AudienceOrg _selectedAudienceOrgType;
        public AudienceOrg SelectedAudienceOrgType
        {
            get
            {
                return _selectedAudienceOrgType;
            }

            set
            {
                if(_selectedAudienceOrgType != value)
                {
                    _selectedAudienceOrgType = value;
                    OnPropertyChanged("SelectedAudienceOrgType");
                }
            }
        }

        public string SelectedAudienceOrgTypeName
        {
            get
            {
                if (this._selectedAudienceOrgType != null)
                {
                    return this._selectedAudienceOrgType.AudienceOrgName;
                }
                return "";

            }
            set
            {
                if (AudienceOrgTypes == null)
                {
                    return;
                }
                var aut = this.AudienceOrgTypes.FirstOrDefault(x => x.AudienceOrgName == value);

                this._selectedAudienceOrgType = aut;
                OnPropertyChanged("SelectedAudienceOrgType");
                OnPropertyChanged("SelectedAudienceOrgTypeName");
            }
        }



        public List<AudienceType> AudienceTypes { get; set; }

        public List<SecondaryTechnology> SecondaryTechnologies { get; set; }

        public ObservableCollection<City> Cities { get; set; }

        private List<AudienceOrg> _audienceOrgTypes;

        public List<AudienceOrg> AudienceOrgTypes
        {
            get { return _audienceOrgTypes; }
            set
            {

                if (SelectedAudienceOrgType != null &&
                    value.FirstOrDefault(x => x.AudienceOrgId == SelectedAudienceOrgType.AudienceOrgId) != null)
                {
                    var audOrg = this.SelectedAudienceOrgType;
                    _audienceOrgTypes = value;
                    OnPropertyChanged("AudienceOrgTypes");
                    this._selectedAudienceOrgType = audOrg;
                    OnPropertyChanged("SelectedAudienceOrgType");
                }
                else
                {
                    _audienceOrgTypes = value;
                    OnPropertyChanged("AudienceOrgTypes");
                }
                
            }
        }

        public List<AudienceOrg> AllAudienceOrgTypes { get; set; }

        public async Task<bool> CheckDataAndShowMessage()
        {
            if (this.Cities == null || this.Cities.Count == 0
                || this.AudienceTypes == null || this.AudienceTypes.Count == 0
                || this.AllAudienceOrgTypes == null || this.AllAudienceOrgTypes.Count == 0
                || this.SecondaryTechnologies == null || this.SecondaryTechnologies.Count == 0)
            {
                await MessageHelper.ShowMessage("Unable to connect with server. Please check your network connection and try again.");
                return false;
            }
            return true;
        }

        public async Task<bool> GetTechnologes()
        {
            if (NetworkHelper.IsNetworkAvailable() == false)
            {
                await MessageHelper.ShowMessage("You require an active network connection to internet for registration");
                this.ShowError = true;
                return false;
            }

            try
            {
                this.OperationInProgress = true;

                // Fetch data about the existing user and assign it to view model.
                var checkProfileResponse = await ServiceProxy.CallService("api/CheckProfile", JsonConvert.SerializeObject(new CheckProfileRequest()
                {
                    AuthProvider = this.AuthProvider,
                    AuthProviderUserId = this.AuthProviderUserId
                }));

                if (checkProfileResponse.IsSuccess)
                {
                    CheckProfileResponse authUserInfoResponse = JsonConvert.DeserializeObject<CheckProfileResponse>(checkProfileResponse.response);

                    this.AudienceTypes = authUserInfoResponse.AudienceTypes;
                    this.AllAudienceOrgTypes = authUserInfoResponse.AudienceOrgTypes;
                    this.SecondaryTechnologies = authUserInfoResponse.SecondaryTechnologies;
                    this.Cities = new ObservableCollection<City>(authUserInfoResponse.Cities);
                    if (this.Cities ==null)
                    {
                        this.Cities = new ObservableCollection<City>();
                    }
                    else if (this.Cities.Count > 0)
                    {
                        this.City = this.Cities[0];
                    }

                    if (this.AudienceTypes == null)
                    {
                        this.AudienceTypes = new List<AudienceType>();
                    }
                    else if (this.AudienceTypes.Count > 0)
                    {
                        this.SelectedAudienceType = this.AudienceTypes[0];
                    }


                    if (this.AudienceOrgTypes == null)
                    {
                        this.AudienceTypes = new List<AudienceType>();
                    }
                    else if (this.AudienceOrgTypes.Count > 0)
                    {
                        this.SelectedAudienceOrgType = this.AudienceOrgTypes[0];
                    }



                    this.OnPropertyChanged("AudienceTypes");
                    this.OnPropertyChanged("SecondaryTechnologies");
                    this.OnPropertyChanged("Cities");

                    if (authUserInfoResponse.IsRegistered)  // check if user is exsting or not
                    {
                        this.FullName = authUserInfoResponse.FullName;
                        this.Email = authUserInfoResponse.Email;
                        this.AuthProvider = authUserInfoResponse.AuthProvider;
                        this.AuthProviderUserId = authUserInfoResponse.AuthProviderUserId;
                        this.City = authUserInfoResponse.City;
                        this.Town = authUserInfoResponse.Town;
                        this.Location = authUserInfoResponse.Location;
                        this.SelectedAudienceType = authUserInfoResponse.SelectedAudienceType;
                        this.SelectedAudienceOrgType = authUserInfoResponse.SelectedAudienceOrgType;
                        this.DeviceId = authUserInfoResponse.DeviceId;
                        this.UserId = authUserInfoResponse.UserId;

                        if(!string.IsNullOrEmpty(this.DeviceId))
                        {
                            this.PushEnabled = true;
                        }

                        await LocalStorage.SaveJsonToFile(this, "registration");
                        return true;
                    }
                    else
                    {
                        await this.SetCity(await GeoLocationHelper.GetCity(this.Cities.ToList()));
                    }
                }
                //else
                //{
                //    // if fetch data : FAILED.
                //    //unable to register you... unable to save your profile...
                //}
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                this.OperationInProgress = false;
            }
            return false;
        }

        public bool IsValid = true;

        public async Task<bool> SaveUserRegitration()
        {
            this.IsValid = true;

            if(string.IsNullOrEmpty(this.FullName))
            {
                await MessageHelper.ShowMessage("Please provide your Name");
                IsValid = false;
                return false;
            }

            else if (this.City == null)
            {
                await MessageHelper.ShowMessage("Please choose your City");
                IsValid = false;
                return false;
            }

            else if(string.IsNullOrEmpty(this.Email))
            {
                await MessageHelper.ShowMessage("Please provide your Email");
                IsValid = false;
                return false;
            }

            else if (this.SelectedAudienceType == null)
            {
                await MessageHelper.ShowMessage("What describes you best?");
                IsValid = false;
                return false;
            }

            else if (this.SelectedAudienceOrgType == null)
            {
                await MessageHelper.ShowMessage("Please select your Organization");
                IsValid = false;
                return false;
            }

            else if(this.SecondaryTechnologies.FirstOrDefault(x => x.IsSelected) == null)
            {
                await MessageHelper.ShowMessage("Please select your Technologies you are interested in");
                IsValid = false;
                return false;
            }



            string email = this.Email;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (!match.Success)
            {
                await MessageHelper.ShowMessage("Please enter valid email address");
                IsValid = false;
                return false;
            }


            try
            {
                this.OperationInProgress = true;

                var pf = new ProfileRequest()
                {
                    FullName = this.FullName,
                    Email = this.Email,
                    AuthProvider = this.AuthProvider,
                    AuthProviderUserId = this.AuthProviderUserId,
                    City = this.City,
                    SelectedAudienceType = this.SelectedAudienceType,
                    SelectedAudienceOrgType = this.SelectedAudienceOrgType,
                    SecondaryTechnologies = this.SecondaryTechnologies,
                    DeviceId = this.DeviceId,
                    DevicePlatform = NotificationsHelper.Platform,
                    PushEnabled = this.PushEnabled,
                    PushId = await NotificationsHelper.GetPushId(),
                    Town = this.Town,
                    Location = this.Location

                }
                ;

                // Fetch data about the user
                //saves and updates data on server
                var result =
                    await ServiceProxy.CallService("api/Profile", JsonConvert.SerializeObject(pf));

                if (result.IsSuccess)
                {
                    ProfileResponse response = JsonConvert.DeserializeObject<ProfileResponse>(result.response);

                    if (response.UserId != 0) // check if user is exsting or not ,,, if 0 means update failed.
                    {
                        this.DeviceId = response.DeviceId;
                        this.UserId = response.UserId.ToString();
                        await LocalStorage.SaveJsonToFile(this, "registration");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                this.OperationInProgress = false;
            }

            return false;
        }

        public async Task SetCity(LocationHelper locationHelper)
        {
            if (locationHelper.FoundCity == locationHelper.FoundTown)
            {
                var foundCity = this.Cities.FirstOrDefault(x => x.CityName == locationHelper.FoundTown);
                if (foundCity != null)
                {
                    this.City = foundCity;
                }
            }
            else
            {
                var foundCity = this.Cities.FirstOrDefault(x => x.CityName == locationHelper.FoundCity);
                if (foundCity != null)
                {
                    this.City = foundCity;
                }
            }
            if (string.IsNullOrEmpty(locationHelper.FoundTown))
            {
                this.Town = locationHelper.FoundTown;
            }
            if (locationHelper.UserLocation != null)
            {
                this.Location = locationHelper.UserLocation;
            }

            OnPropertyChanged("ShowSeperateLocation");
        }
    }
}

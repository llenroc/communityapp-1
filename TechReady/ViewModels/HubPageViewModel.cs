using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TechReady.Common.DTO;
using TechReady.Common.Models;
using TechReady.Helpers.MessageHelper;
using TechReady.Helpers.NetworkHelper;
using TechReady.Helpers.ServiceCallers;
using TechReady.Helpers.Storage;
using TechReady.Models;
using TechReady.NavigationParameters;
using TechReady.ServiceCallers;

namespace TechReady.ViewModels
{
    public class HubPageViewModel : BaseViewModel
    {

        public EventsFilterPageNavigationParameters CurrentFilterCriteria { get; set; }
        public LearningResourcesFilterPageNavigationParameters CurrentLearningResourcesFilterCriteria { get; set; }

        private bool _speakersTileList = true;

        public bool SpeakersTileList
        {
            get
            {
               return this._speakersTileList;
            }

            set
            {
                if(this._speakersTileList != value)
                {
                    this._speakersTileList = value;
                    OnPropertyChanged("SpeakersTileList");
                } 
            }
        }
        public ObservableCollection<TrackSpeaker> Only8SpeakersList  {get; set;}

        public void AddOnlyLimitedSpeakers(List<TrackSpeaker> list, int count)
        {
            if (Only8SpeakersList == null)
                Only8SpeakersList = new ObservableCollection<TrackSpeaker>();
            Only8SpeakersList.Clear();

            list.Reverse();

            foreach(var item in list)
            {
                Only8SpeakersList.Add(item);
                if (Only8SpeakersList.Count == count)
                    break;
            }
            OnPropertyChanged("Only8SpeakersList");
        }

        private string _selectedLocation;

        public string SelectedLocation
        {
            get { return _selectedLocation; }
            set
            {
                if (_selectedLocation != value)
                {
                    _selectedLocation = value;
                    OnPropertyChanged("SelectedLocation");
                }
            }
        }


        private string _selectedType;

        public string SelectedType
        {
            get { return _selectedType; }
            set
            {
                if (_selectedType != value)
                {
                    _selectedType = value;
                    OnPropertyChanged("SelectedType");
                }
            }
        }

        private string _selectedTechnology;

        public string SelectedTechnology
        {
            get { return _selectedTechnology; }
            set
            {
                if (_selectedTechnology != value)
                {
                    _selectedTechnology = value;
                    OnPropertyChanged("SelectedTechnology");


                }
            }
        }

        private string _selectedRole;

        public string SelectedRole
        {
            get { return _selectedRole; }
            set
            {
                if (_selectedRole != value)
                {
                    _selectedRole = value;
                    OnPropertyChanged("SelectedRole");

                }
            }
        }



        public bool AreEventsAvailable
        {
            get
            {
                if (this.OperationInProgress == false && this.AllEvents != null && this.AllEvents.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool AreLearningResourcesAvailable
        {
            get
            {
                if (this.OperationInProgress == false && this.AllLearningResources != null &&
                    this.AllLearningResources.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool AreNotificationsAvailable
        {
            get
            {
                if (this.OperationInProgress == false && this.AllShownNotifications != null && this.AllShownNotifications.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private ObservableCollection<Event> _allEvents;

        public ObservableCollection<Event> AllEvents
        {
            get { return _allEvents; }
            set
            {
                if (this._allEvents != value)
                {
                    this._allEvents = value;
                    OnPropertyChanged("AllEvents");
                    OnPropertyChanged("AreEventsAvailable");
                }
            }
        }

        private bool _showAll = false;

        public bool ShowAll
        {
            get { return _showAll; }
            set
            {
                _showAll = value;
                OnPropertyChanged("ShowAll");
            }
        }

     


        private bool _resetFilterVisible = false;

        public bool ResetFilterVisible
        {
            get { return _resetFilterVisible; }
            set
            {
                _resetFilterVisible = value;
                OnPropertyChanged("ResetFilterVisible");
            }
        }


        private ObservableCollection<Event> _recommendedEvents;

        public ObservableCollection<Event> RecommendedEvents
        {
            get { return _recommendedEvents; }
            set
            {
                if (this._recommendedEvents != value)
                {
                    this._recommendedEvents = value;
                    OnPropertyChanged("RecommendedEvents");
                }
            }
        }


        private ObservableCollection<Notification> _allNotifications;
        public ObservableCollection<Notification> AllNotifications
        {
            get
            {
                return _allNotifications;
            }
            set
            {
                if (_allNotifications != value)
                {
                    _allNotifications = value;
                    OnPropertyChanged("AllNotifications");
                    OnPropertyChanged("AllShownNotifications");
                    OnPropertyChanged("AreNotificationsAvailable");
                }
            }
        }

        public override bool OperationInProgress
        {
            get { return base.OperationInProgress; }
            set
            {
                base.OperationInProgress = value;
                OnPropertyChanged("AreEventsAvailable");
                OnPropertyChanged("AreNotificationsAvailable");
                OnPropertyChanged("AreLearningResourcesAvailable");
                
            }
        }

        public ObservableCollection<Notification> AllShownNotifications     //list of notifications which are to be shown to the user
        {
            get
            {
                if (this.AllNotifications == null)
                {
                    return null;
                }
                var _allShownNotifications = new ObservableCollection<Notification>(AllNotifications.Where(x => x.Removed == false).ToList());
                return _allShownNotifications;
                //return new ObservableCollection<Notification>(AllNotifications.Where(x => x.Removed == false).ToList());
            }
        }
        public async Task GetEvents()
        {
            try
            {
                this.OperationInProgress = true;

                ObservableCollection<Event> model = await LocalStorage.ReadJsonFromFile<ObservableCollection<Event>>("allevents");
                await SetEvents(model);

                if (NetworkHelper.IsNetworkAvailable() == false)
                {
                    if (model == null)
                    {
                        await MessageHelper.ShowMessage("Please connect to internet to download latest events");
                        return;
                    }
                }
                else
                {
                    HomeRequest request = new HomeRequest()
                    {

                    };

                    var result = await ServiceProxy.CallService("api/Events", JsonConvert.SerializeObject(request));

                    if (result.IsSuccess)
                    {
                        var homeResponse = JsonConvert.DeserializeObject<HomeResponse>(result.response);

                        model = homeResponse.AllEvents;
                        await LocalStorage.SaveJsonToFile(homeResponse.AllEvents, "allevents");


                    }
                    else
                    {
                        //await MessageHelper.ShowMessage(result.Error.Message);
                    }

                }

                await SetEvents(model);
            }
            catch
            {
            }
            finally
            {
                this.OperationInProgress = false;
            }
        }

        private async Task SetEvents(ObservableCollection<Event> model)
        {
            try
            {
                if (model != null)
                {
                    var usermodel = await LocalStorage.ReadJsonFromFile<UserRegistrationPageViewModel>("registration");

                    if (usermodel != null)
                    {
                        var list = new ObservableCollection<Event>();

                        foreach (var ev in model)
                        {
                            try
                            {
                                if (usermodel.SelectedAudienceType.AudienceTypeName == "Student" && 
                                    (ev.EventType == "Firstparty_ProDev") && ev.EventRoles.Contains(usermodel.SelectedAudienceType.AudienceTypeName)==false)
                                {
                                    continue;
                                }
                                ev.Weightage = 1;

                                if (ev.GlobalEvent)
                                {
                                    ev.Weightage = 10;
                                    continue;
                                }

                                if (ev.EventType == "Webinar" &&
                                    ev.EventRoles.Contains(usermodel.SelectedAudienceType.AudienceTypeName))
                                {
                                    bool found = false;
                                    foreach (var tech in ev.EventTechnologies)
                                    {
                                        if (
                                            usermodel.SecondaryTechnologies.FirstOrDefault(
                                                x => x.IsSelected && x.PrimaryTechnologyName == tech) != null)
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                    if (found)
                                    {
                                        ev.Weightage = 5;
                                    }
                                    else
                                    {
                                        ev.Weightage = 4;
                                    }
                                    continue;

                                }

                                if (ev.EventRoles.Contains(usermodel.SelectedAudienceType.AudienceTypeName) &&
                                    ev.CityName == usermodel.City.CityName)
                                {
                                    bool found = false;
                                    foreach (var tech in ev.EventTechnologies)
                                    {
                                        if (
                                            usermodel.SecondaryTechnologies.FirstOrDefault(
                                                x => x.IsSelected && x.PrimaryTechnologyName == tech) != null)
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                    if (found)
                                    {
                                        ev.Weightage = 9;
                                    }
                                    else
                                    {
                                        ev.Weightage = 8;
                                    }
                                    continue;
                                }

                                if (ev.EventRoles.Contains(usermodel.SelectedAudienceType.AudienceTypeName))
                                {
                                    bool found = false;
                                    foreach (var tech in ev.EventTechnologies)
                                    {
                                        if (
                                            usermodel.SecondaryTechnologies.FirstOrDefault(
                                                x => x.IsSelected && x.PrimaryTechnologyName == tech) != null)
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                    if (found)
                                    {
                                        ev.Weightage = 7;
                                    }
                                    else
                                    {
                                        ev.Weightage = 6;
                                    }
                                    continue;
                                }
                            }
                            catch (Exception)
                            {


                            }

                        }

                        model = new ObservableCollection<Event>(model.OrderByDescending(x => x.Weightage));

                            
                        this.AllEvents = model;

                        ObservableCollection<Event> recommendedmodel = null;

                        if (AllEvents.Count > 5)
                        {
                            recommendedmodel = new ObservableCollection<Event>(AllEvents.Take(5));
                            recommendedmodel.Add(new LoadMore());
                        }
                        else
                        {
                            recommendedmodel = AllEvents;
                        }
                        this.RecommendedEvents = recommendedmodel;
                    }
                }
            }
            catch (Exception)
            {
                
                
            }
            
        }


        public async Task GetNotifications()
        {
            this.OperationInProgress = true;
            try
            {

                ObservableCollection<Notification> model = null;
                if (NetworkHelper.IsNetworkAvailable() == false)
                {
                    model = await LocalStorage.ReadJsonFromFile<ObservableCollection<Notification>>("allNotifications");
                    this.AllNotifications = model;
                    //await MessageHelper.ShowMessage("Please connect to internet to download latest events");
                    return;

                }
                else
                {
                 

                    // Fetch data about the user
                    //saves and updates data on server
                    var userModel = await LocalStorage.ReadJsonFromFile<UserRegistrationPageViewModel>("registration");
                    if (userModel != null)
                    {

                        //TODO SHIV: API Check
                        var result = await ServiceProxy.CallService("api/Notifications", JsonConvert.SerializeObject(
                    new NotificationsRequest()
                    {
                        AppUserId = userModel.UserId
                    }));

                        if (result.IsSuccess)
                        {
                            NotificationsResponse response = JsonConvert.DeserializeObject<NotificationsResponse>(result.response);
                            model = await LocalStorage.ReadJsonFromFile<ObservableCollection<Notification>>("allNotifications");
                            if (model != null)
                            {
                                foreach (var notif in model)
                                {
                                    //response.UserNotifications.FirstOrDefault(n => (n.NotificationID == notif.NotificationID && notif.Read));
                                    if (notif.Read)
                                    {
                                        if (response.UserNotifications.FirstOrDefault(n => (n.NotificationID == notif.NotificationID)) != null)
                                        {
                                            response.UserNotifications.FirstOrDefault(n => (n.NotificationID == notif.NotificationID)).Read = true;
                                            this.Notification_Tapped(response.UserNotifications.FirstOrDefault(n => (n.NotificationID == notif.NotificationID)));
                                        }
                                    }
                                    if (notif.Removed)
                                    {
                                        if (response.UserNotifications.FirstOrDefault(n => (n.NotificationID == notif.NotificationID)) != null)
                                        {
                                            response.UserNotifications.FirstOrDefault(n => (n.NotificationID == notif.NotificationID)).Removed = true;
                                            this.RemoveNotification_Click(response.UserNotifications.FirstOrDefault(n => (n.NotificationID == notif.NotificationID)));
                                        }
                                    }
                                }
                            }
                            this.AllNotifications = new ObservableCollection<Notification>(response.UserNotifications.Where(x=>x.Removed==false).ToList().OrderByDescending(x=>x.PushDateTime));
                            await LocalStorage.SaveJsonToFile(this.AllNotifications, "allNotifications");
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            finally
            {
                this.OperationInProgress = false;
            }
        }

        


        public async Task FilterEvents(string location, string technology, string role)
        {
            this.SelectedLocation = location ?? "";
            this.SelectedRole = role ?? "";
            this.SelectedTechnology = technology ?? "";

            ObservableCollection<Event> model;
            model = await LocalStorage.ReadJsonFromFile<ObservableCollection<Event>>("allevents");
            if (model == null)
            {
                await MessageHelper.ShowMessage("Please connect to internet to download latest events");
                return;
            }

            var list = (from c in model
                where
                    (this.SelectedTechnology == "" || SelectedTechnology == "All" ||
                     c.EventTechnologies.Contains(SelectedTechnology))
                    && (SelectedRole == "" || SelectedRole == "All" || c.EventRoles.Contains(SelectedRole))
                    && (SelectedLocation == "" || SelectedLocation == "All" ||
                        c.CityName == SelectedLocation)
                select c).ToList();

            this.AllEvents = new ObservableCollection<Event>(list);
            this.ShowAll = true;
        }


        public async Task FilterLearningResources(string type, string technology, string role)
        {
            this.SelectedType = type ?? "";
            this.SelectedRole = role ?? "";
            this.SelectedTechnology = technology ?? "";

            ObservableCollection<LearningResource> model;
            model = await LearningResourceService.GetLearningResourcesFromLocal();
            if (model == null)
            {
                await MessageHelper.ShowMessage("Please connect to internet to download learning resources");
                return;
            }

            var list = (from c in model
                where
                    (this.SelectedTechnology == "" || SelectedTechnology == "All" ||
                     c.PrimaryTechnologyName == this.SelectedTechnology)
                    && (SelectedRole == "" || SelectedRole == "All" || c.AudienceTypes.FirstOrDefault(x=>x.AudienceTypeName== SelectedRole)!=null)
                    && (SelectedType == "" || SelectedType == "All" ||
                        c.LearningResourceType == SelectedType)
                select c).ToList();

            this.AllLearningResources = new ObservableCollection<LearningResource>(list);
            this.ShowAll = true;
        }




        public async Task ResetFiltersEvent()
        {
            this.SelectedLocation = "";
            this.SelectedRole = "";
            this.SelectedTechnology = "";

            //if (NetworkHelper.IsNetworkAvailable() == false)    // only in case of no network scenario
            //    await this.FilterEvents(this.SelectedLocation, this.SelectedTechnology, this.SelectedRole);
            this.GetEvents();

            this.ShowAll = false;
            
        }
        public async Task ResetFiltersLearningResources()
        {
            this.SelectedType = "";
            this.SelectedRole = "";
            this.SelectedTechnology = "";

            //if (NetworkHelper.IsNetworkAvailable() == false)    // only in case of no network scenario
                //await this.FilterLearningResources(this.SelectedType, this.SelectedTechnology, this.SelectedRole);
            this.GetLearningContent();

            this.ShowAll = false;

        }

        public async Task GetLearningContent()
        {
            this.OperationInProgress = true;
            try
            {
                LearningResourcesRequest request = new LearningResourcesRequest()
                {
                    RequestedPageNo = 1,
                    SourceType = "All",
                    Technologies = new List<string>(),
                    UserRole = "All"
                };


                ObservableCollection<LearningResource> model =
                    await LearningResourceService.GetLearningResourcesFromLocal();

                await SetLearningContent(model);

                if (NetworkHelper.IsNetworkAvailable() == false)
                {
                    if (model == null)
                    {
                        await MessageHelper.ShowMessage("Please connect to internet to download learning resources");
                        return;
                    }
                }
                else
                {

                    var result = await LearningResourceService.GetLearningResourcesFromServer(request);

                    if (result != null)
                    {
                        model = result;
                        await LearningResourceService.SaveLearningResources(model);
                    }
                    else
                    {
                        //await MessageHelper.ShowMessage(result.Error.Message);
                    }


                }

                var favVideos =
                    await LocalStorage.ReadJsonFromFile<ObservableCollection<LearningResource>>("watchedVideos");

                foreach (var flr in favVideos)
                {
                    var lr = model.Where(x => x.LearningResourceID == flr.LearningResourceID).FirstOrDefault();
                    if (lr != null)
                    {
                        lr.Favourited = true;
                    }
                }
                await SetLearningContent(model);

            }
            catch (Exception)
            {


            }
            finally
            {
                this.OperationInProgress = false;
            }

        }

        private async Task SetLearningContent(ObservableCollection<LearningResource> model)
        {
            if (model != null)
            {
                var usermodel = await LocalStorage.ReadJsonFromFile<UserRegistrationPageViewModel>("registration");

                if (usermodel != null)
                {
                    foreach (LearningResource rs in model)
                    {
                        if (
                            rs.AudienceTypes.FirstOrDefault(
                                x => x.AudienceTypeName == usermodel.SelectedAudienceType.AudienceTypeName) != null)
                        {
                            rs.Weitage++;
                        }

                        if (
                            usermodel.SecondaryTechnologies.FirstOrDefault(
                                x => x.PrimaryTechnologyName == rs.PrimaryTechnologyName) != null)
                        {
                            rs.Weitage++;
                        }
                    }

                    model = new ObservableCollection<LearningResource>(model.OrderByDescending(x => x.Weitage));

                    await LearningResourceService.SaveLearningResources(model);

                    this.AllLearningResources = model;

                    var recomendedresources =
                        new ObservableCollection<LearningResource>(model.OrderByDescending(x => x.Weitage).ToList());

                    if (AllLearningResources.Count > 5)
                    {
                        recomendedresources = new ObservableCollection<LearningResource>(recomendedresources.Take(5));
                        recomendedresources.Add(new LoadMoreLearningResource());
                    }
                    else
                    {
                        recomendedresources = AllLearningResources;
                    }

                    this.RecommendedLearningResources = recomendedresources;

                    OnPropertyChanged("AllLearningResources");
                    OnPropertyChanged("RecommendedLearningResources");
                }
            }
        }


        private ObservableCollection<LearningResource> _recommendedLearningResources;

        public ObservableCollection<LearningResource> RecommendedLearningResources
        {
            get { return _recommendedLearningResources; }
            set
            {
                if (_recommendedLearningResources != value)
                {
                    _recommendedLearningResources = value;
                    OnPropertyChanged("RecommendedLearningResources");
                }
            }
        }


        private ObservableCollection<LearningResource> _allLearningResources;

        public ObservableCollection<LearningResource> AllLearningResources
        {
            get { return _allLearningResources; }
            set
            {
                if (_allLearningResources != value)
                {
                    _allLearningResources = value;
                    OnPropertyChanged("AllLearningResources");
                    OnPropertyChanged("AreLearningResourcesAvailable");
                }
            }
        }
        public async void Notification_Tapped(object sender)
        {
            try
            {
                var notification = sender as Notification;
                notification.Read = true;
                //this.AllNotifications.FirstOrDefault(n => n.NotificationID == notification.NotificationID).Read = true;
                OnPropertyChanged("Read");

                if (NetworkHelper.IsNetworkAvailable() == false)
                {
                    await LocalStorage.SaveJsonToFile(this.AllNotifications, "allNotifications");
                    return;
                }

                var model = await LocalStorage.ReadJsonFromFile<UserRegistrationPageViewModel>("registration");
                if (model != null)
                {
                    if (Helpers.NetworkHelper.NetworkHelper.IsNetworkAvailable() == true)
                    {
                        //TODO SHIV: API Check
                        var result = await ServiceProxy.CallService("api/MarkNotification", JsonConvert.SerializeObject(new TechReady.Common.DTO.MarkNotificationRequest()
                        {
                            AppUserId = model.UserId,
                            NotificationId = notification.NotificationID.ToString()
                        }));

                        if (result.IsSuccess)
                        {
                            //model.SaveUserRegitration();
                        }
                    }
                }

                await LocalStorage.SaveJsonToFile(this.AllNotifications, "allNotifications");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception thrown at HUB PAGE NOTIFICATION TAPPED " + ex.Message);
            }
            finally
            {

            }
        }
        public async void RemoveNotification_Click(Notification notification)
        {
            //notification.Removed = true;
            //OnPropertyChanged("Removed");

            //service call on removed item from item and savedd it locally
            try
            {
                //if (this.AllNotifications != null && this.AllNotifications.Count > 0 && this.AllNotifications.Contains(notification))
                //{
                //    this.AllNotifications.Remove(notification);
                //    OnPropertyChanged("AllNotifications");
                //    OnPropertyChanged("AreNotificationsAvailable");
                //}

                notification.Removed = true;
                if ((this.AllNotifications.FirstOrDefault(x => x.NotificationID == notification.NotificationID) != null))
                {
                    (this.AllNotifications.FirstOrDefault(x => x.NotificationID == notification.NotificationID)).Removed = true;
                }
                OnPropertyChanged("Removed");
                OnPropertyChanged("AllNotifications");
                OnPropertyChanged("AllShownNotifications");
                OnPropertyChanged("AreNotificationsAvailable");

                if (NetworkHelper.IsNetworkAvailable() == false)
                {
                    await LocalStorage.SaveJsonToFile(this.AllNotifications, "allNotifications");
                    return;
                }

                var model = await LocalStorage.ReadJsonFromFile<UserRegistrationPageViewModel>("registration");
                if (model != null)
                {
                    if (Helpers.NetworkHelper.NetworkHelper.IsNetworkAvailable() == true)
                    {
                        //TODO SHIV: API Check
                        var result = await ServiceProxy.CallService("api/RemoveNotification", JsonConvert.SerializeObject(new TechReady.Common.DTO.MarkNotificationRequest()
                        {
                            AppUserId = model.UserId,
                            NotificationId = notification.NotificationID.ToString()
                        }));

                        if (result.IsSuccess)
                        {
                            //model.SaveUserRegitration();
                        }
                    }
                }

                await LocalStorage.SaveJsonToFile(this.AllNotifications, "allNotifications");
            }
            catch
            {
            }

        }

    }
}
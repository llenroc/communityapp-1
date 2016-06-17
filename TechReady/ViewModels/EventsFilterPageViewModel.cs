using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
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

namespace TechReady.ViewModels
{
    class EventsFilterPageViewModel :BaseViewModel
    {
        private ObservableCollection<Event> _allEvents;

        public ObservableCollection<Event> AllEvents
        {
            get
            {
                return _allEvents;
            }
            set
            {
                if (this._allEvents != value)
                {
                    this._allEvents = value;
                    OnPropertyChanged("AllEvents");
                }
            }
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
                    if (value != null)
                    {
                        this.RefreshLists("SelectedLocation");
                    }
                }
            }
        }

        public void RefreshLists(string changedProperty)
        {
            var location = "";
            var technology = "";
            var role = "";

            switch (changedProperty)
            {
                case "SelectedLocation":
                    if (this.Roles.Contains(this.selectedRole))
                    {
                        role = this.SelectedRole;
                        OnPropertyChanged("Roles");
                        this._selectedRole = role;
                        OnPropertyChanged("SelectedRole");
                    }
                    else
                    {
                        OnPropertyChanged("Roles");
                    }

                    if (this.Technologies.Contains(this.selectedTechnology))
                    {
                        technology = this.SelectedTechnology;
                        OnPropertyChanged("Technologies");
                        this._selectedTechnology = technology;
                        OnPropertyChanged("SelectedTechnology");

                    }
                    else
                    {
                        OnPropertyChanged("Technologies");
                    }
                    break;

                case "SelectedRole":

                    if (this.Locations.Contains(this.selectedLocation) )
                    {
                        location = this.SelectedLocation;
                        OnPropertyChanged("Locations");
                        this._selectedLocation = location;
                        OnPropertyChanged("SelectedLocation");
                    }
                    else
                    {
                        OnPropertyChanged("Locations");
                    }
                    if (this.Technologies.Contains(this.selectedTechnology))
                    {
                        technology = this.SelectedTechnology;
                        OnPropertyChanged("Technologies");
                        this._selectedTechnology = technology;
                        OnPropertyChanged("SelectedTechnology");
                    }
                    else
                    {
                        OnPropertyChanged("Technologies");

                    }
                    break;

                case "SelectedTechnology":
                    if (this.Locations.Contains(this.selectedLocation))
                    {
                        location = this.SelectedLocation;
                        OnPropertyChanged("Locations");
                        this._selectedLocation = location;
                        OnPropertyChanged("SelectedLocation");
                    }
                    else
                    {
                        OnPropertyChanged("Locations");
                    }
                    if (this.Roles.Contains(this.selectedRole))
                    {
                        role = this.SelectedRole;
                        OnPropertyChanged("Roles");
                        this._selectedRole = role;
                        OnPropertyChanged("SelectedRole");
                    }
                    else
                    {
                        OnPropertyChanged("Roles");
                    }
                    break;

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

                    if (value != null)
                    {
                        this.RefreshLists("SelectedTechnology");
                    }
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
                    if (value != null)
                    {
                        this.RefreshLists("SelectedRole");
                    }
                }
            }
        }

        private string selectedLocation
        {
            get
            {
                string location = "All";
                if (!string.IsNullOrEmpty(this.SelectedLocation) && this.SelectedLocation != "All")
                {
                    location = this.SelectedLocation;
                }
                return location;
            }
        }

        private string selectedTechnology
        {
            get
            {
                string technology = "All";
                if (!string.IsNullOrEmpty(this.SelectedTechnology) && this.SelectedTechnology != "All")
                {
                    technology = this.SelectedTechnology;
                }
                return technology;
            }
        }

        private string selectedRole
        {
            get
            {
                string role = "All";
                if (!string.IsNullOrEmpty(this.SelectedRole) && this.SelectedRole != "All")
                {
                    role = this.SelectedRole;
                }

                return role;
            }
        }

        public List<string> Locations
        {
            get
            {
                if (this.AllEvents == null)
                {
                    return new List<string>()
                    {
                        "All"
                    };
                }

                var list = (from c in this.AllEvents
                    where
                        (selectedTechnology == "" || selectedTechnology == "All" ||
                         c.EventTechnologies.Contains(selectedTechnology))
                        && (selectedRole == "" || selectedRole == "All" || c.EventRoles.Contains(selectedRole))
                    select c.CityName).Distinct().ToList();

                list.Add("All");

                return list;
            }
        }

        public List<string> Technologies
        {
            get
            {
                if (this.AllEvents == null)
                {
                    return new List<string>()
                    {
                        "All"
                    };
                }

                var list = (from c in this.AllEvents
                    where
                        (selectedLocation == "" || selectedLocation == "All" ||
                         c.CityName == selectedLocation)
                        && (selectedRole == "" || selectedRole == "All" || c.EventRoles.Contains(selectedRole))
                    select c.EventTechnologies).ToList();

                var stringList = new List<string>();
                foreach (var item in list)
                {
                    foreach (var it in item)
                    {
                        stringList.Add(it);
                    }
                }

                stringList.Add("All");

                return stringList.Distinct().ToList();
            }
        }

        public List<string> Roles
        {
            get
            {
                if (this.AllEvents == null)
                {
                    return new List<string>()
                    {
                        "All"
                    };
                }

                var list = (from c in this.AllEvents
                            where
                                (selectedLocation == "" || selectedLocation == "All" ||
                                 c.CityName == selectedLocation)
                                && (selectedTechnology == "" || selectedTechnology == "All" ||
                         c.EventTechnologies.Contains(selectedTechnology))
                            select c.EventRoles).Distinct().ToList();


                var stringList = new List<string>();
                foreach (var item in list)
                {
                    foreach (var it in item)
                    {
                        stringList.Add(it);
                    }
                }

                stringList.Add("All");

                return stringList.Distinct().ToList();
            }
        }


        public async Task GetEvents()
        {
            ObservableCollection<Event> model;
            this.AllEvents = await LocalStorage.ReadJsonFromFile<ObservableCollection<Event>>("allevents");
            OnPropertyChanged("Technologies");
            OnPropertyChanged("Locations");
            OnPropertyChanged("Roles");
        }
    }
}
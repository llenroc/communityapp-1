using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TechReady.Common.Models;
using TechReady.Models;
using TechReady.Helpers.ServiceCallers;
using TechReady.ServiceCallers;

namespace TechReady.ViewModels
{
    class FilterLearningResourcesPageViewModel :BaseViewModel
    {
        private ObservableCollection<LearningResource> _allResources;

        public ObservableCollection<LearningResource> AllResources
        {
            get
            {
                return _allResources;
            }
            set
            {
                if (this._allResources != value)
                {
                    this._allResources = value;
                    OnPropertyChanged("AllResources");
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
                case "SelectedType":
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

                    if (this.Types.Contains(this.selectedType))
                    {
                        location = this.SelectedType;
                        OnPropertyChanged("Types");
                        this._selectedType = location;
                        OnPropertyChanged("SelectedType");
                    }
                    else
                    {
                        OnPropertyChanged("Types");
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
                    if (this.Types.Contains(this.selectedType))
                    {
                        location = this.SelectedType;
                        OnPropertyChanged("Types");
                        this._selectedType = location;
                        OnPropertyChanged("SelectedType");
                    }
                    else
                    {
                        OnPropertyChanged("Types");
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
                    if (value != null)
                    {
                        this.RefreshLists("SelectedType");
                    }
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

        private string selectedType
        {
            get
            {
                string type = "All";
                if (!string.IsNullOrEmpty(this.SelectedType) && this.SelectedType != "All")
                {
                    type = this.SelectedType;
                }
                return type;
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

        public List<string> Types
        {
            get
            {
                if (this.AllResources == null)
                {
                    return new List<string>()
                    {
                        "All"
                    };
                }

                var list = (from c in this.AllResources
                    where
                        (selectedTechnology == "" || selectedTechnology == "All" ||
                         string.IsNullOrEmpty(c.PrimaryTechnologyName) || c.PrimaryTechnologyName== selectedTechnology)
                        && (selectedRole == "" || selectedRole == "All" || c.AudienceTypes.FirstOrDefault(x=>x.AudienceTypeName == selectedRole)!=null)
                    select c.LearningResourceType).Distinct().ToList();

                list.Add("All");

                return list;
            }
        }

        public List<string> Technologies
        {
            get
            {
                if (this.AllResources == null)
                {
                    return new List<string>()
                    {
                        "All"
                    };
                }

                var list = (from c in this.AllResources
                    where
                        (selectedType == "" || selectedType == "All" ||
                         string.IsNullOrEmpty(c.LearningResourceType) || c.LearningResourceType == selectedType)
                        &&
                        (selectedRole == "" || selectedRole == "All" || c.AudienceTypes.FirstOrDefault(x => x.AudienceTypeName == selectedRole) != null)
                    select c.PrimaryTechnologyName).ToList();


                list.Add("All");

                return list.Distinct().ToList();
            }
        }

        public List<string> Roles
        {
            get
            {
                if (this.AllResources == null)
                {
                    return new List<string>()
                    {
                        "All"
                    };
                }

                var list1 = (from c in this.AllResources
                    where
                        (selectedTechnology == "" || selectedTechnology == "All" ||
                         string.IsNullOrEmpty(c.PrimaryTechnologyName) || c.PrimaryTechnologyName == selectedTechnology)
                        && (selectedType == "" || selectedType == "All" ||
                            string.IsNullOrEmpty(c.LearningResourceType) || c.LearningResourceType == selectedType)
                    select c.AudienceTypes).ToList();


                

                var list = new List<String>();
                foreach (var at in list1)
                {
                    foreach (var att in at)
                    {
                        list.Add(att.AudienceTypeName);
                    }
                }
                list.Add("All");

                return list.Distinct().ToList();
            }
        }


        public async Task GetLearningResources()
        {
            this.AllResources = await LearningResourceService.GetLearningResourcesFromLocal();
            OnPropertyChanged("Technologies");
            OnPropertyChanged("Types");
            OnPropertyChanged("Roles");
        }
    }
}
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TechReady.API.DTO;
using TechReady.Common.Models;
using TechReady.Helpers.NetworkHelper;
using TechReady.Helpers.ServiceCallers;
using TechReady.Helpers.Storage;
using TechReady.Models;

namespace TechReady.ViewModels
{
    public class WatchedVideosListPageViewModel : BaseViewModel
    {


        public ObservableCollection<LearningResource> Resources
        {
            get
            {
                return _resources;
            }

            set
            {
                if (this._resources != value)
                {
                    this._resources = value;
                    OnPropertyChanged("Resources");
                    OnPropertyChanged("HasResources");
                }
            }

        }

        private ObservableCollection<LearningResource> _resources { get; set; }

        public bool HasResources
        {
            get
            {
                if(this.Resources != null && this.Resources.Count > 0)
                    return true;
                return false;
            }
        }

        public async Task GetWatchedVideos()
        {
            try
            {
                this.OperationInProgress = true;

                var model = await LocalStorage.ReadJsonFromFile<ObservableCollection<LearningResource>>("watchedVideos");

                this.Resources = this.SortByDate(model);
            }
            catch (System.Exception ex)
            {

            }
            finally
            {
                this.OperationInProgress = false;
            }
        }

        public ObservableCollection<LearningResource> SortByDate(ObservableCollection<LearningResource> list)
        {
            if (list == null)
            {
                return new ObservableCollection<LearningResource>();
            }
            System.Collections.Generic.List<LearningResource> newList = new System.Collections.Generic.List<LearningResource>();
            foreach (var item in list)
            {
                newList.Add(item);
            }
            newList.Sort((x, y) => x.LastWatchedTime.CompareTo(y.LastWatchedTime));
            newList.Reverse();
            list.Clear();
            foreach(var item in newList)
            {
                list.Add(item);
            }
            return list;
        }
    }
}

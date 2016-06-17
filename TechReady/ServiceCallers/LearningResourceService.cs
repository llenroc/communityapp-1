using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TechReady.Common.DTO;
using TechReady.Helpers.ServiceCallers;
using TechReady.Helpers.Storage;
using TechReady.Models;

namespace TechReady.ServiceCallers
{
    public class LearningResourceService
    {
        public static async Task<ObservableCollection<LearningResource>> GetLearningResourcesFromServer(
            LearningResourcesRequest request)
        {
            try
            {
                var result =
                    await ServiceProxy.CallService("api/LearningResources", JsonConvert.SerializeObject(request));

                if (result.IsSuccess)
                {
                    var homeResponse = JsonConvert.DeserializeObject<LearningResourcesResponse>(result.response);
                    return homeResponse.LearningResources;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task SaveLearningResources(ObservableCollection<LearningResource> learningResources)
        {
           await LocalStorage.SaveJsonToFile(learningResources, "LearningResources");
        }

        public static async Task<ObservableCollection<LearningResource>> GetLearningResourcesFromLocal()
        {
            return await LocalStorage.ReadJsonFromFile<ObservableCollection<LearningResource>>("LearningResources");
        }
    }
}
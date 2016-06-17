using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TechreadyForms.Helpers.Storage;
using Xamarin.Forms;

namespace TechReady.Helpers.Storage
{
    public class LocalStorage
    {
        public static Task SaveJsonToFile<T>(T objectToSave, string fileName)
        {
            return Task.Factory.StartNew(() =>
            {
                var service = DependencyService.Get<ISaveAndLoad>();

                service.SaveText(fileName, JsonConvert.SerializeObject(objectToSave, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));

            });

        }

        public static Task<T> ReadJsonFromFile<T>(string fileName) where T : class
        {
            T jsonObject = null;


            Task<T> t =  Task.Factory.StartNew<T>(() =>
            {
                try
                {
                    var service = DependencyService.Get<ISaveAndLoad>();

                    var fileText = service.LoadText(fileName);

                    jsonObject = JsonConvert.DeserializeObject<T>(fileText);

                    return jsonObject;
                }
                catch (Exception ex)
                {
                    return null;
                }
            });

            return t;
        }

        public static Task DeleteJsonFromFile(string fileName)
        {

            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var service = DependencyService.Get<ISaveAndLoad>();

                    service.Delete(fileName);

                }
                catch
                {

                }
            });
        
        }
    }
}

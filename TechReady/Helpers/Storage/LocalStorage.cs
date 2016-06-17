using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;

namespace TechReady.Helpers.Storage
{
    public class LocalStorage
    {
        public static async Task SaveJsonToFile<T>(T objectToSave, string fileName)
        {
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;

            var file = await folder.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.ReplaceExisting);
            if (file != null)
            {
                var textToWrite =
                    JsonConvert.SerializeObject(objectToSave, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                await Windows.Storage.FileIO.WriteTextAsync(file, textToWrite);
            }

        }

        public static async Task<T> ReadJsonFromFile<T>(string fileName) where T : class
        {
            T jsonObject = null;
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;

            try
            {
                var file = await folder.GetFileAsync(fileName);
                if (file != null)
                {
                    var fileText = await Windows.Storage.FileIO.ReadTextAsync(file);
                    jsonObject = JsonConvert.DeserializeObject<T>(fileText);
                }
            }
            catch (Exception ex)
            {

            }
            return jsonObject;
        }

        public static async Task DeleteJsonFromFile(string fileName)
        {
           
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;

            try
            {
                var file = await folder.GetFileAsync(fileName);
                if (file != null)
                {
                    await file.DeleteAsync();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private static List<string> _allFiles = new List<string>()
        {
            "registration",
            "LearningResources",
            "followedEvents",
            "allevents",
            "allNotifications",
            "followedSpeakers",
            "allspeakers",
            "watchedVideos"
        };

        public static async Task RemoveAllFiles()
        {

            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;

            try
            {
                foreach (string _file in _allFiles)
                {
                    try {
                        var file = await folder.GetFileAsync(_file);
                        if (file != null)
                        {
                            await file.DeleteAsync();
                        }
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine("DEBUG: File \'"+_file+"\' not Deleted. EXCEPTION: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception thrown at removing all files from local storage "+ex.Message);
            }
        }
    }
}

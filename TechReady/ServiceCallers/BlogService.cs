using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Html;
using TechReady.Models;
using TNX.RssReader;
using TechReady.Helpers.ExtensionProperties;

namespace TechReady.ServiceCallers
{
    public class BlogService
    {
        public static async Task<ObservableCollection<LearningResource>> GetBlogs(string url)
        {
            try
            {
                RssFeed feed = null;

                await Task.Factory.StartNew(
                    () =>
                    {
                        try
                        {
                            feed =
                                RssHelper.ReadFeed(new Uri(url));
                        }
                        catch (Exception ex)
                        {

                        }

                    });



                ObservableCollection<LearningResource> learningResources = new ObservableCollection<LearningResource>();


                if (feed == null)
                {
                    return null;
                }
                foreach (var item in feed.Items)
                {
                    try
                    {
                        LearningResource v = new LearningResource();
                        v.Description = HtmlUtilities.ConvertToText(item.Description)
                            .Replace("\n", "")
                            .Replace("\r", "");
                        v.Link = item.Link;
                        v.Title = item.Title;
                        v.PublicationTime = item.PublicationUtcTime;
                        learningResources.Add(v);
                    }
                    catch (Exception)
                    {
                        //
                    }
                    
                }
                return learningResources;
            }
            catch (Exception ex)
            {
                return null;
            }


        }

        //public static async Task SaveVideos(ObservableCollection<Video> videos)
        //{
        //    await LocalStorage.SaveJsonToFile<ObservableCollection<Video>>(videos, "videos");
        //}

        //public static async Task<ObservableCollection<Video>> GetVideosFromLocal()
        //{
        //    return await LocalStorage.ReadJsonFromFile<ObservableCollection<Video>>("videos");
        //}
    }
}

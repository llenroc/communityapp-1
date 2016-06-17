using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Azure.WebJobs;
using TechReady.Portal.Models;

namespace FetchLearningResources
{
 
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    public class Program
    {
        public static T GetExtensionElementValue<T>(SyndicationItem item, string extensionElementName)
        {
            return item.ElementExtensions.First(ee => ee.OuterName == extensionElementName).GetObject<T>();
        }

        public static string GetThumbnailChannel9(SyndicationItem item)
        {
            foreach (XElement s in item.ElementExtensions.ReadElementExtensions<XElement>("thumbnail", "http://search.yahoo.com/mrss/"))
            {
                return s.FirstAttribute.Value;
            }
            return "";
        }

        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        private static void Main()
        {
            using (TechReadyDbContext ctx = new TechReadyDbContext())
            {
                var feedsToCheck = (from c in ctx.LearningResourceFeeds
                    select c).ToList();

                foreach (var feedType in feedsToCheck)
                {
                    XmlReader reader = XmlReader.Create(feedType.RSSLink,
                        new XmlReaderSettings()
                        {
                        });

                    SyndicationFeed feed = SyndicationFeed.Load(reader);

                    foreach (SyndicationItem i in feed.Items)
                    {
                        var link = i.Links[0].Uri.ToString();
                        if (ctx.LearningResources.FirstOrDefault(x => x.ContentURL == link) == null)
                        {
                            try
                            {
                                XmlSyndicationContent content = i.Content as XmlSyndicationContent;
                                LearningResource lr = new LearningResource();


                                lr.Title = i.Title.Text;
                                lr.AudienceTypes = new List<AudienceType>();
                                foreach (AudienceType a in feedType.AudienceTypes)
                                {
                                    lr.AudienceTypes.Add(a);
                                }
                                
                                lr.ContentURL = i.Links[0].Uri.ToString();
                                lr.LearningResourceTypeID = feedType.LearningResourceTypeID;
                                lr.PrimaryTechnologyID = feedType.PrimaryTechnologyID;
                                lr.PublicationTime =
                                    i.PublishDate.UtcDateTime;

                                if (feedType.LearningResourceTypeID == 1)
                                {
                                    lr.Description = GetExtensionElementValue<string>(i, "summary");
                                    lr.ThumbnailURL = GetThumbnailChannel9(i);
                                }
                                else
                                {
                                    lr.Description = i.Summary.Text;
                                    lr.ThumbnailURL = "";
                                }
                                ctx.LearningResources.Add(lr);

                            }
                            catch
                            {
                                
                            }

                        }
                    }

                }
                ctx.SaveChanges();
            }
        }
    }
}
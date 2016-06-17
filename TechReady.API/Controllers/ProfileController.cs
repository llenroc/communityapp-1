using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Azure.NotificationHubs;
using TechReady.API.DTO;
using TechReady.API.Models;
using DbModel = TechReady.Portal.Models;
using TechReady.Common.DTO;
using TechReady.Common.Models;
using System.Threading.Tasks;
using TechReady.Portal.Helpers;
using Microsoft.Azure.NotificationHubs.Messaging;

namespace TechReady.API.Controllers
{
    public class ProfileController : ApiController
    {
        private NotificationHubClient hub;

        public ProfileController()
        {
            hub = NotificationsHubHelper.Hub;
        }



        [System.Web.Mvc.HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody]ProfileRequest request)
        {
            HttpResponseMessage response;
           try
           {
               var profileResponse = new ProfileResponse();

                using (  DbModel.TechReadyDbContext ctx = new   DbModel.TechReadyDbContext())
                {
                    var appUser = (from c in ctx.AppUsers
                        where
                            c.AuthProviderName == request.AuthProvider &&
                            c.AuthProviderUserId == request.AuthProviderUserId
                        select c).FirstOrDefault();

                    if (appUser == null)
                    {
                        appUser = new   DbModel.AppUser();
                        ctx.AppUsers.Add(appUser);
                        appUser.RegistrationDateTime = DateTime.Now;
                    }

                    appUser.CityName = request.City.CityName;
                    appUser.Town = request.Town;
                    if (request.Location != null)
                    {
                        appUser.Location = ConvertGeoCode(request.Location); 
                    }
                    appUser.AudienceOrgID = request.SelectedAudienceOrgType.AudienceOrgId;
                    appUser.AuthProviderName = request.AuthProvider;
                    appUser.AuthProviderUserId = request.AuthProviderUserId;
                    appUser.Email = request.Email;
                    appUser.FullName = request.FullName;




             

                    if (appUser.TechnologyTags == null)
                    {
                        appUser.TechnologyTags = new List<  DbModel.PrimaryTechnology>();
                    }
                    
                    foreach (var tech in request.SecondaryTechnologies)
                    {
                        var c = (from t in ctx.PrimaryTechnologies
                                 where t.PrimaryTechnologyID == tech.PrimaryTechnologyId
                                 select t).FirstOrDefault();
                        if (c != null)
                        {
                            if (tech.IsSelected)
                            {
                                appUser.TechnologyTags.Add(c);
                            }
                            else
                            {
                                if (
                                    appUser.TechnologyTags.FirstOrDefault(
                                        x => x.PrimaryTechnologyID == c.PrimaryTechnologyID) != null)
                                {
                                    appUser.TechnologyTags.Remove(c);
                                }
                            }
                        }
                    }
                    ctx.SaveChanges();
                    if (request.PushEnabled && !string.IsNullOrEmpty(request.PushId))
                    {
                        //Create/Update if the Platform Changes or Notifications are Enabled
                        if ((appUser.DevicePlatform != request.DevicePlatform) 
                            || (appUser.PushEnabled!=request.PushEnabled) 
                            || string.IsNullOrEmpty(appUser.DeviceId)
                            || appUser.PushId != request.PushId
                            )
                        {
                            appUser.DeviceId = await hub.CreateRegistrationIdAsync();

                            RegistrationDescription registration = null;
                            switch (request.DevicePlatform)
                            {
                                case "mpns":
                                    registration = new MpnsRegistrationDescription(request.PushId);
                                    break;
                                case "wns":
                                    registration = new WindowsRegistrationDescription(request.PushId);
                                    break;
                                case "apns":
                                    registration = new AppleRegistrationDescription(request.PushId);
                                    break;
                                case "gcm":
                                    registration = new GcmRegistrationDescription(request.PushId);
                                    break;
                                default:
                                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                            }
                            registration.RegistrationId = appUser.DeviceId;

                            registration.Tags =
                                new HashSet<string>(appUser.TechnologyTags.Select(x => x.PrimaryTech).ToList().Select(x=>x.Replace(" ","_")).ToList());
                            registration.Tags.Add(request.SelectedAudienceType.AudienceTypeName.Replace(" ","_"));
                            registration.Tags.Add("userId:" + appUser.AppUserID);

                            if (appUser.FollowedEvents != null)
                            {
                                foreach (var followedEvent in appUser.FollowedEvents)
                                {
                                    registration.Tags.Add("eventId:" + followedEvent.EventID.ToString());
                                }
                            }
                            if (appUser.FollowedSpeakers != null)
                            {
                                foreach (var followedSpeaker in appUser.FollowedSpeakers)
                                {
                                    registration.Tags.Add("speakerId:" + followedSpeaker.SpeakerID.ToString());
                                }
                            }


                            await hub.CreateOrUpdateRegistrationAsync(registration);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(appUser.DeviceId))
                        {
                            await hub.DeleteRegistrationAsync(appUser.DeviceId);
                            appUser.DeviceId = null;
                        }
                    }

                    appUser.DevicePlatform = request.DevicePlatform;
                    appUser.LastAccessTime = DateTime.Now;
                    appUser.PushEnabled = request.PushEnabled;
                    appUser.PushId = request.PushId;

                    ctx.SaveChanges();

                    profileResponse.UserId =
                        appUser.AppUserID;
                }
                response = this.Request.CreateResponse(HttpStatusCode.OK, profileResponse);
                response.Content.Headers.Expires = new DateTimeOffset(DateTime.Now.AddSeconds(300));
            }
            catch (Exception ex)
            {
                HttpError myCustomError = new HttpError(ex.Message) { { "IsSuccess", false } };
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, myCustomError);
            }
            return response;
        }

        private static TrackSpeaker CreateDxSpeaker(  DbModel.Speaker ev)
        {
            var speaker = new TrackSpeaker()
            {
                Affiliation = ev.Affiliation.ToString(),
                Email = ev.Email,
                PicUrl = ev.PicUrl,
                Profile = ev.Profile,
                SpeakerId = ev.SpeakerID,
                SpeakerName = ev.SpeakerName,
                SpeakerType = ev.Type.ToString(),
                Title = ev.Title,
                BlogLink = ev.BlogLink,
                FacebookLink = ev.FacebookLink,
                LinkedinLink = ev.LinkedInLink,
                Location = ev.Location,
                TwitterLink = ev.TwitterHandle,
                SpeakerEvents = new ObservableCollection<Event>()
            };

            //foreach (var trackAgenda in ev.TrackAgendas)
            //{
            //    var evt = trackAgenda.EventTrack.Event;
            //    if (speaker.SpeakerEvents!=null && speaker.SpeakerEvents.FirstOrDefault(x=>x.EventId==evt.EventID) == null)
            //    {
            //        speaker.SpeakerEvents.Add(EventsController.CreateDxEvent(evt));
            //    }
            //}

            
            return speaker;
        }

        public static DbGeography ConvertGeoCode(GeoCodeItem geoCode, int srid = 4326)
        {
            var text = $"POINT({geoCode.Longitude} {geoCode.Latitude})";
            return DbGeography.PointFromText(text, srid);

        }

    }
}
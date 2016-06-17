using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using TechReady.API.DTO;
using TechReady.API.Models;
using DbModel = TechReady.Portal.Models;
using TechReady.Common.DTO;
using TechReady.Common.Models;

namespace TechReady.API.Controllers
{
    public class CheckProfileController : ApiController
    {
        [System.Web.Mvc.HttpPost]
        public HttpResponseMessage Post([FromBody]CheckProfileRequest request)
        {
            HttpResponseMessage response;
           try
           {
               var technologiesResponse = TechnologiesController.CreateTechnologiesResponse();

               CheckProfileResponse checkProfileResponse = new CheckProfileResponse()
               {
                   Cities = technologiesResponse.Cities,
                   AudienceOrgTypes = technologiesResponse.AudienceOrgTypes,
                   AudienceTypes = technologiesResponse.AudienceTypes,
                   SecondaryTechnologies = technologiesResponse.SecondaryTechnologies,
               };

                using (DbModel.TechReadyDbContext ctx = new DbModel.TechReadyDbContext())
                {
                    var appUser = (from c in ctx.AppUsers
                        where
                            c.AuthProviderName == request.AuthProvider &&
                            c.AuthProviderUserId == request.AuthProviderUserId
                        select c).FirstOrDefault();

                    if (appUser != null)
                    {
                        checkProfileResponse.City = new City()
                        {
                            CityName = appUser.CityName,
                            
                        };
                        if (appUser.City.Location != null)
                        {
                            checkProfileResponse.City.Location = new GeoCodeItem()
                            {
                                Latitude = appUser.City.Location.Latitude,
                                Longitude = appUser.City.Location.Longitude
                            };
                        }

                        if (appUser.Location != null)
                        {
                            checkProfileResponse.Location = new GeoCodeItem()
                            {
                                Latitude = appUser.Location.Latitude,
                                Longitude = appUser.Location.Longitude
                            };
                        }

                        checkProfileResponse.Town = appUser.Town;
                        
                        checkProfileResponse.AuthProvider = request.AuthProvider;
                        checkProfileResponse.AuthProviderUserId = request.AuthProviderUserId;
                        checkProfileResponse.Email = appUser.Email;
                        checkProfileResponse.FullName = appUser.FullName;
                        checkProfileResponse.IsRegistered = true;
                        checkProfileResponse.UserId = appUser.AppUserID.ToString();
                        checkProfileResponse.DeviceId = appUser.DeviceId;
                        checkProfileResponse.SelectedAudienceOrgType = new AudienceOrg()
                        {
                            AudienceTypeName = appUser.AudienceOrg.AudienceType1.TypeOfAudience,
                            AudienceOrgId = appUser.AudienceOrg.AudienceOrgID,
                            AudienceOrgName = appUser.AudienceOrg.AudOrg
                        };
                        checkProfileResponse.SelectedAudienceType = new AudienceType()
                        {
                            AudienceTypeName = appUser.AudienceOrg.AudienceType1.TypeOfAudience,
                            AudienceTypeId = appUser.AudienceOrg.AudienceType1.AudienceTypeID
                        };

                        foreach (var tech in appUser.TechnologyTags)
                        {
                            var PrimaryTech =
                                checkProfileResponse.SecondaryTechnologies.FirstOrDefault(
                                    x => x.PrimaryTechnologyId == tech.PrimaryTechnologyID);

                            if (PrimaryTech != null)
                            {
                                PrimaryTech.IsSelected = true;
                            }
                        }

                    }

                }
                response = this.Request.CreateResponse(HttpStatusCode.OK, checkProfileResponse);
                response.Content.Headers.Expires = new DateTimeOffset(DateTime.Now.AddSeconds(300));
            }
            catch (Exception ex)
            {
                HttpError myCustomError = new HttpError(ex.Message) { { "IsSuccess", false } };
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, myCustomError);
            }
            return response;
        }

        private static TrackSpeaker CreateDxSpeaker(DbModel.Speaker ev)
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
    }
}
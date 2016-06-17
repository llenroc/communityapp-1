using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using DbModel = TechReady.Portal.Models;
using TechReady.Common.DTO;
using TechReady.Common.Models;

namespace TechReady.API.Controllers
{
    public class EventsController : ApiController
    {
        [System.Web.Mvc.HttpPost]
        public HttpResponseMessage Post([FromBody]HomeRequest request)
        {
            HttpResponseMessage response;
           try
            {
                HomeResponse homeResponse = new HomeResponse();
                homeResponse.AllEvents = new ObservableCollection<TechReady.Common.Models.Event>();

                using (DbModel.TechReadyDbContext ctx = new DbModel.TechReadyDbContext())
                {
                    var events = (from c in ctx.Events
                        where (c.EventStatus == DbModel.EventStatus.Tentative || c.EventStatus == DbModel.EventStatus.Confirmed) &&
                              c.EventVisibility == DbModel.EventVisibility.Visible
                        select c).ToList();

               
                    foreach (var ev in events)
                    {
                        try
                        {

                            if (ev.EventToDate.HasValue &&
                                (DateTime.Now.Date - ev.EventToDate.Value.Date).TotalDays > 1)
                            {
                                continue;
                            }

                            homeResponse.AllEvents.Add(CreateDxEvent(ev));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Trace.WriteLine(ex.Message);
                        }
                        
                    }
                }

                response = this.Request.CreateResponse(HttpStatusCode.OK, homeResponse);
                response.Content.Headers.Expires = new DateTimeOffset(DateTime.Now.AddSeconds(300));
            }
            catch (Exception ex)
            {
                HttpError myCustomError = new HttpError(ex.Message) { { "IsSuccess", false } };
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, myCustomError);
            }
            return response;
        }

        public static Event CreateDxEvent(DbModel.Event ev)
        {

            Event dxEvent = new Event()
            {
                EventStatus = ev.EventStatus.ToString(),
                EventFromDate = ev.EventFromDate,
                EventToDate = ev.EventToDate,
                EventAbstract = ev.EventAbstract,
                CityName = ev.CityName,
                EventId = ev.EventID,
                EventName = ev.EventName,
                EventRegLink = ev.RegLink,
                EventType = ev.EventType.ToString(),
                EventVenue = ev.EventVenue,
                Tracks = new List<EventTrack>(),
                GlobalEvent = ev.IsGlobal,
                EventTechnologies = new List<string>(),
                EventRoles = new List<string>()
            };

          foreach (var tech in ev.EventTechnologTags)
                {
                    dxEvent.EventTechnologies.Add(tech.PrimaryTech);
                }

            foreach (var at in ev.EventAudienceTypeTags)
            {
                dxEvent.EventRoles.Add(at.TypeOfAudience);
            }


            foreach (var et in ev.EventTracks)
            {

                var dxEventTrack = new EventTrack()
                {
                    AudienceType = et.Track.AudType.TypeOfAudience,
                    AudienceTypeId = et.Track.AudienceTypeID,
                    EventTrackId = et.EventTrackID,
                    Format = et.Track.Format.ToString(),
                    TrackAbstract = et.Track.TrackAbstract,
                    TrackEndTime = et.TrackEndTime,
                    TrackStartTime = et.TrackStartTime,
                    TrackId = et.Track.TrackID,
                    TrackVenue = et.TrackVenue,
                    TrackDisplayName = et.Track.TrackDisplayName,
                    Sessions = new List<TrackSession>()
                };
                dxEvent.Tracks.Add(dxEventTrack);


                foreach (var ets in et.Track.Session)
                {
                    var eta = (from c in et.TrackAgendas
                        where c.SessionID == ets.SessionID
                        select c).FirstOrDefault();

                    if (eta == null)
                    {
                        continue;
                    }

                    var dxEventSession = new TrackSession()
                    {
                        Abstract = ets.Abstract,
                        Posrequisites = ets.PostRequisites,
                        Prerequisites = ets.PreRequisites,
                        PrimaryTechnologyId = ets.PrimaryTechnologyID,
                        PrimaryTechnologyName = ets.PrimaryTechnology.PrimaryTech,
                        Product = ets.Product,
                        SecondaryTechnologyId = ets.SecondaryTechnologyID,
                        SecondaryTechnologyName = ets.SecondaryTechnology.SecondaryTech,
                        SessionEndTime = eta.EndTime,
                        SessionNo = ets.SessionNo,
                        SessionStartTime = eta.StartTime,
                        Speaker = new TrackSpeaker()
                        {
                            Affiliation = eta.Speaker.Affiliation.ToString(),
                            Email = eta.Speaker.Email,
                            PicUrl = eta.Speaker.PicUrl,
                            Profile = eta.Speaker.Profile,
                            SpeakerId = eta.SpeakerID,
                            SpeakerName = eta.Speaker.SpeakerName,
                            SpeakerType = eta.Speaker.Type.ToString(),
                            Title = eta.Speaker.Title,
                            BlogLink = eta.Speaker.BlogLink,
                            FacebookLink = eta.Speaker.FacebookLink,
                            LinkedinLink = eta.Speaker.LinkedInLink,
                            Location = eta.Speaker.Location,
                            TwitterLink = eta.Speaker.TwitterHandle
                        },
                        Title = ets.Title,
                        TechLevel = ets.TechLevel,
                        TrackAgendaId = eta.TrackAgendaID
                    };

                    dxEventTrack.Sessions.Add(dxEventSession);

                    if (dxEvent.EventTechnologies.Contains(ets.PrimaryTechnology.PrimaryTech) == false)
                    {
                        dxEvent.EventTechnologies.Add(ets.PrimaryTechnology.PrimaryTech);
                    }

                    
                }

                if (dxEvent.EventRoles.Contains(dxEventTrack.AudienceType) == false)
                {
                    dxEvent.EventRoles.Add(dxEventTrack.AudienceType);
                }

            }

            return dxEvent;
        }
    }
}
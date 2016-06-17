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
using DbModel = TechReady.Portal.Models;
using TechReady.Common.DTO;
using TechReady.Common.Models;

namespace TechReady.API.Controllers
{
    public class SpeakersController : ApiController
    {
        [System.Web.Mvc.HttpPost]
        public HttpResponseMessage Post([FromBody]SpeakersRequest request)
        {
            HttpResponseMessage response;
           try
            {
                SpeakersResponse speakersResponse = new SpeakersResponse();
                speakersResponse.AllSpeakers = new ObservableCollection<TechReady.Common.Models.TrackSpeaker>();

                using (DbModel.TechReadyDbContext ctx = new DbModel.TechReadyDbContext())
                {
                    var speakers = (from c in ctx.Speakers
                        select c).ToList();

                    foreach (var speaker in speakers)
                    {
                        try
                        {
                            speakersResponse.AllSpeakers.Add(CreateDxSpeaker(speaker));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Trace.WriteLine(ex.Message);
                        }
                        
                    }
                }

                response = this.Request.CreateResponse(HttpStatusCode.OK, speakersResponse);
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
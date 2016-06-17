using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public class SpeakerEventsController : ApiController
    {
        [System.Web.Mvc.HttpPost]
        public HttpResponseMessage Post([FromBody]SingleSpeakerRequest request)
        {
            HttpResponseMessage response;
           try
            {
                SpeakerEventsResponse speakersResponse = new SpeakerEventsResponse();
                speakersResponse.SpeakerEvents = new ObservableCollection<TechReady.Common.Models.Event>();

                using (DbModel.TechReadyDbContext ctx = new DbModel.TechReadyDbContext())
                {
                    var speakers = (from c in ctx.Speakers
                        where c.SpeakerID == request.SpeakerId
                        select c).FirstOrDefault();

                    speakersResponse.SpeakerEvents =
                    CreateDxSpeaker(speakers);
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


        private static ObservableCollection<Event> CreateDxSpeaker(DbModel.Speaker ev)
        {
            var
                SpeakerEvents = new ObservableCollection<Event>();


            foreach (var trackAgenda in ev.TrackAgendas)
            {
                var evt = trackAgenda.EventTrack.Event;
                if (SpeakerEvents.FirstOrDefault(x => x.EventId == evt.EventID) == null)
                {
                    try
                    {
                        if (evt.EventToDate.HasValue &&
                             (DateTime.Now.Date - evt.EventToDate.Value.Date).TotalDays > 1)
                        {
                            continue;
                        }
                        SpeakerEvents.Add(EventsController.CreateDxEvent(evt));
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }

            return SpeakerEvents;
        }
    }
}
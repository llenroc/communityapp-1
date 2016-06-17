using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TechReady.Portal.Models;
using TechReady.Common.DTO;

namespace TechReady.API.Controllers
{
    public class FollowEventController : ApiController
    {
        [System.Web.Mvc.HttpPost]
        public HttpResponseMessage Post([FromBody]FollowEventRequest request)
        {
            HttpResponseMessage response;
            int appUserId;
            int eventId;
            try
            {
                FollowEventResponse followEventResponse = new FollowEventResponse();
                appUserId = Convert.ToInt32 (request.AppUserId);
                eventId = Convert.ToInt32(request.EventId);
                using (TechReady.Portal.Models.TechReadyDbContext ctx = new TechReady.Portal.Models.TechReadyDbContext())
                {
                    var appuser = (from c in ctx.AppUsers
                                   where c.AppUserID == appUserId
                                   select c).FirstOrDefault();

                    if (appuser != null)
                    {
                        var ev = (from c in ctx.Events
                                      where c.EventID == eventId
                                      select c).FirstOrDefault();

                        if (appuser.FollowedEvents == null)
                        {
                            appuser.FollowedEvents = new List<Event>();
                        }
                        if (request.Follow)
                        {
                            if (ev != null)
                            {
                                appuser.FollowedEvents.Add(ev);
                            }
                        }
                        else
                        {
                            appuser.FollowedEvents.Remove(ev);
                        }
                        ctx.SaveChanges();
                    }
                }

                response = this.Request.CreateResponse(HttpStatusCode.OK, followEventResponse);
                response.Content.Headers.Expires = new DateTimeOffset(DateTime.Now.AddSeconds(300));
            }
            catch (Exception ex)
            {
                HttpError myCustomError = new HttpError(ex.Message) { { "IsSuccess", false } };
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, myCustomError);
            }
            return response;
        }

    }
}


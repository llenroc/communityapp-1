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
    public class FollowSpeakerController : ApiController
    {
        [System.Web.Mvc.HttpPost]
        public HttpResponseMessage Post([FromBody]FollowSpeakerRequest request)
        {
            HttpResponseMessage response;
            int appUserId;
            int speakerId;
            try
            {
                FollowSpeakerResponse followSpeakerResponse = new FollowSpeakerResponse();
                appUserId = Convert.ToInt32(request.AppUserId);
                speakerId = Convert.ToInt32(request.SpeakerId);
                using (TechReady.Portal.Models.TechReadyDbContext ctx = new TechReady.Portal.Models.TechReadyDbContext())
                {
                    var appuser = (from c in ctx.AppUsers
                                   where c.AppUserID == appUserId
                                   select c).
                    FirstOrDefault();

                    if (appuser != null)
                    {
                        var speaker = (from c in ctx.Speakers
                                       where c.SpeakerID == speakerId
                                       select c).FirstOrDefault();

                        if (appuser.FollowedSpeakers == null)
                        {
                            appuser.FollowedSpeakers = new List<Speaker>();
                        }

                        if (request.Follow)
                        {
                            
                            if (speaker != null)
                            {
                                appuser.FollowedSpeakers.Add(speaker);
                            }

                        }
                        else
                        {
                            appuser.FollowedSpeakers.Remove(speaker);
                        }
                        ctx.SaveChanges();
                    }
                }

                response = this.Request.CreateResponse(HttpStatusCode.OK, followSpeakerResponse);
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

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
    public class WatchedLearningResourceController : ApiController
    {
        [System.Web.Mvc.HttpPost]
        public HttpResponseMessage Post([FromBody]WatchedLearningResourceRequest request)
        {
            HttpResponseMessage response;
            int appUserId;
            int learningResourceId;
            try
            {
                WatchedLearningResourceResponse followWatchedLearningResourceResponse = new WatchedLearningResourceResponse();

                appUserId = Convert.ToInt32(request.AppUserId);
                learningResourceId = Convert.ToInt32(request.WatchedLearningResourceId);

                using (TechReady.Portal.Models.TechReadyDbContext ctx = new TechReady.Portal.Models.TechReadyDbContext())
                {
                    var appuser = (from c in ctx.AppUsers
                                   where c.AppUserID == appUserId
                                   select c).
                    FirstOrDefault();

                    if (appuser != null)
                    {
                        var watchedLearningResource = (from c in ctx.LearningResources
                                       where c.LearningResourceID == learningResourceId
                                                       select c).FirstOrDefault();


                        if (appuser.WatchedLearningResources == null)
                        {
                            appuser.WatchedLearningResources = new List<LearningResource>();
                        }
                            if (watchedLearningResource != null)
                            {
                                appuser.WatchedLearningResources.Add(watchedLearningResource);
                                ctx.SaveChanges();
                        }
                    }
                }

                response = this.Request.CreateResponse(HttpStatusCode.OK, followWatchedLearningResourceResponse);
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

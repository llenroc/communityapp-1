using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using TechReady.Portal.Models;
using TechReady.Common.DTO;

namespace TechReady.API.Controllers
{
    public class FeedbackController : ApiController
    {
        [System.Web.Mvc.HttpPost]
        public HttpResponseMessage Post([FromBody]FeedbackRequest request)
        {
            HttpResponseMessage response;
            try
            {
                FeedbackResponse feedbackResponse = new FeedbackResponse();

                using (TechReady.Portal.Models.TechReadyDbContext ctx = new TechReady.Portal.Models.TechReadyDbContext())
                {
                    var appUserId = Convert.ToInt32(request.AppUserId);
                    var appuser  = (from c in ctx.AppUsers
                                   where c.AppUserID == appUserId select c).
                    FirstOrDefault();

                    if (appuser != null)
                    {
                        AppFeedback feedback = new AppFeedback()
                        {
                            AppUserID = appUserId,
                            Email = request.Email,
                            FeedbackText = request.Feedback,
                            FeedbackType =request.FeedbackType,
                            Name = request.Name
                        };

                        ctx.AppFeedbacks.Add(feedback);

                        ctx.SaveChanges();

                        feedbackResponse.ResponseText = "Thank you for sharing your feedback!";
                    }
                    else
                    {
                        feedbackResponse.ResponseText = "User is not registered, Please register to provide feedback";
                    }

                }

                response = this.Request.CreateResponse(HttpStatusCode.OK, feedbackResponse);
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

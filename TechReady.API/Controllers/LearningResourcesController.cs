using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DbModel = TechReady.Portal.Models;
using TechReady.Common.DTO;
using TechReady.Common.Models;
using TechReady.Models;

namespace TechReady.API.Controllers
{
    public class LearningResourcesController : ApiController
    {
        [System.Web.Mvc.HttpPost]
        public HttpResponseMessage Post([FromBody]LearningResourcesRequest request)
        {
            HttpResponseMessage response;
           try
            {
                LearningResourcesResponse learningResourcesResponse = new LearningResourcesResponse
                {
                    LearningResources = new ObservableCollection<LearningResource>()
                };


                using (DbModel.TechReadyDbContext ctx = new DbModel.TechReadyDbContext())
                {
                    var learningResources = (from c in ctx.LearningResources
                        select c).ToList();

                    foreach (var lr in learningResources)
                    {
                        try
                        {
                            learningResourcesResponse.LearningResources.Add(CreateDxLearningResource(lr));
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Trace.WriteLine(ex.Message);
                        }
                        
                    }
                }

                response = Request.CreateResponse(HttpStatusCode.OK, learningResourcesResponse);
                response.Content.Headers.Expires = new DateTimeOffset(DateTime.Now.AddSeconds(300));
            }
            catch (Exception ex)
            {
                HttpError myCustomError = new HttpError(ex.Message) { { "IsSuccess", false } };
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, myCustomError);
            }
            return response;
        }

        private static LearningResource CreateDxLearningResource(DbModel.LearningResource lr)
        {
            LearningResource dxLr = new LearningResource()
            {
                LearningResourceType = lr.LearningResourceType==null?"":lr.LearningResourceType.LearningResourceTypeName,
                Description = lr.Description,
                Title = lr.Title,
                PrimaryTechnologyName = lr.PrimaryTechnology==null?"":lr.PrimaryTechnology.PrimaryTech,
                PublicationTime = lr.PublicationTime,
                LearningResourceID = lr.LearningResourceID,
             
                Link = lr.ContentURL,
                Thumbnail = lr.ThumbnailURL
            };

            dxLr.AudienceTypes = new List<AudienceType>();
            foreach (var at in lr.AudienceTypes)
            {
                dxLr.AudienceTypes.Add(new AudienceType()
                {
                    AudienceTypeId = at.AudienceTypeID,
                    AudienceTypeName = at.TypeOfAudience
                });
            }
            return dxLr;
        }
    }
}
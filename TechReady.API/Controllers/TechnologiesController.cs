using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using TechReady.API.Models;
using DbModel = TechReady.Portal.Models;
using TechReady.Common.DTO;
using TechReady.Common.Models;

namespace TechReady.API.Controllers
{
    public class TechnologiesController : ApiController
    {
        [System.Web.Mvc.HttpPost]
        public HttpResponseMessage Post([FromBody] TechnologiesRequest request)
        {
            HttpResponseMessage response;
            try
            {
                var techResponse = CreateTechnologiesResponse();
                response = this.Request.CreateResponse(HttpStatusCode.OK, techResponse);
                response.Content.Headers.Expires = new DateTimeOffset(DateTime.Now.AddSeconds(300));
            }
            catch (Exception ex)
            {
                HttpError myCustomError = new HttpError(ex.Message) {{"IsSuccess", false}};
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, myCustomError);
            }
            return response;
        }

        public static TechnologiesResponse CreateTechnologiesResponse()
        {
            TechnologiesResponse techResponse = new TechnologiesResponse();
            techResponse.AudienceTypes = new List<AudienceType>();
            techResponse.AudienceOrgTypes = new List<AudienceOrg>();
            techResponse.SecondaryTechnologies = new List<SecondaryTechnology>();
            techResponse.Cities = new List<City>();


            using (DbModel.TechReadyDbContext ctx = new DbModel.TechReadyDbContext())
            {
                var audience = (from c in ctx.AudienceTypes
                    select c).ToList();

                foreach (var ev in audience)
                {
                    try
                    {
                        var at = new AudienceType()
                        {
                            AudienceTypeId = ev.AudienceTypeID,
                            AudienceTypeName = ev.TypeOfAudience
                        };

                        techResponse.AudienceTypes.Add(at);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex.Message);
                    }
                }

                var audienceOrg = (from c in ctx.AudienceOrgs select c).ToList();

                foreach (var at in audienceOrg)
                {
                    try
                    {
                        var ao = new AudienceOrg()
                        {
                            AudienceOrgId = at.AudienceOrgID,
                            AudienceTypeName = at.AudienceType1.TypeOfAudience,
                            AudienceOrgName = at.AudOrg
                        };

                        techResponse.AudienceOrgTypes.Add(ao);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex.Message);
                    }
                }

                var stList = (from c in ctx.PrimaryTechnologies
                    select c).ToList();

                foreach (var st in stList)
                {
                    try
                    {
                        var dxSt = new SecondaryTechnology()
                        {
                            PrimaryTechnologyId = st.PrimaryTechnologyID,
                            SecondaryTechnologyId = st.PrimaryTechnologyID,
                            SecondaryTechnologyName = st.PrimaryTech,
                            PrimaryTechnologyName = st.PrimaryTech
                        };

                        techResponse.SecondaryTechnologies.Add(dxSt);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex.Message);
                    }
                }


                var cl = (from c in ctx.Cities
                    select c).ToList();

                foreach (var c in cl)
                {
                    try
                    {

                        var dxc = new City()
                        {
                            CityName = c.CityName,
                        
                        };
                        if (c.Location != null)
                        {
                            dxc.Location = new GeoCodeItem()
                            {
                                Latitude = c.Location.Latitude,
                                Longitude = c.Location.Longitude
                            };
                        }

                        techResponse.Cities.Add(dxc);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex.Message);
                    }
                }
            }
            return techResponse;
        }
    }
}
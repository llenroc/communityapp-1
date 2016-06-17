using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using Autofac.Core.Lifetime;
using AutoMapper;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using TechReady.Services.DataObjects;
using TechReady.Services.Models;

namespace TechReady.Services.Controllers
{
    public class UserProfileController : TableController<UserProfileDTO>
    {
        private MobileServiceContext context;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = new MobileServiceContext();

            context.Database.Log = s =>
            {
                Debug.WriteLine(s.ToString());
            };

            DomainManager = new SimpleMappedEntityDomainManager<UserProfileDTO, UserProfile>(context, Request, Services,
                userProfile => userProfile.Id);
        }

        // GET tables/TodoItem
        public IQueryable<UserProfileDTO> GetAllProfiles()
        {
            var user = User as ServiceUser;
            return Query();
        }

        // GET tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<UserProfileDTO> GetUserProfile(string id)
        {
            //return

            //    Mapper.Map<UserProfile, UserProfileDTO>(
            //        await
            //            this.context.UserProfiles.Include("InterestedIn").Where(x => x.Id == id).FirstOrDefaultAsync());
            return Lookup(id);
        }

        // PATCH tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public async Task<UserProfileDTO> PatchUserProfile(string id, Delta<UserProfileDTO> patch)
        {
            UserProfile currentUserProfile = this.context.UserProfiles.Include("InterestedIn")
                .First(j => (j.Id == id));


            UserProfileDTO updatedUserProfileEntity = patch.GetEntity();

            ICollection<TechnologyDTO> updatedTechnologies;

            bool reqeustContainsRelatedEntities = patch.GetChangedPropertyNames().Contains("InterestedIn");

            if (reqeustContainsRelatedEntities)
            {
                //If request contains Items get the updated list from the patch
                Mapper.Map<UserProfileDTO, UserProfile>(updatedUserProfileEntity, currentUserProfile);
                updatedTechnologies = updatedUserProfileEntity.InterestedIn;
            }
            else
            {
                //If request doest not have Items, then retain the original association
                UserProfileDTO userProfileDTOUpdated = Mapper.Map<UserProfile, UserProfileDTO>
                    (currentUserProfile);
                patch.Patch(userProfileDTOUpdated);
                Mapper.Map<UserProfileDTO, UserProfile>(userProfileDTOUpdated, currentUserProfile);
                updatedTechnologies = userProfileDTOUpdated.InterestedIn;
            }

            if (updatedTechnologies != null)
            {
                //Update related Items
                currentUserProfile.InterestedIn = new List<Technology>();
                foreach (var currentTechnologyDTO in updatedTechnologies)
                {
                    //Look up existing entry in database
                    Technology existingItem = this.context.Technologies
                                .FirstOrDefault(j => (j.Id.ToString() == currentTechnologyDTO.Id.ToString()));

                    if (existingItem != null)
                    {
                        //Convert client type to database type
                        //            Mapper.Map<TechnologyDTO, Technology>(currentTechnologyDTO,
                        //              existingItem);
                        currentUserProfile.InterestedIn.Add(existingItem);
                    }
                }
            }

            await this.context.SaveChangesAsync();

            //Convert to client type before returning the result
            var result = Mapper.Map<UserProfile, UserProfileDTO>(currentUserProfile);
            return result;

        }

        // POST tables/TodoItem
        public async Task<IHttpActionResult> PostUserProfile(UserProfileDTO item)
        {
            UserProfileDTO current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteUserProfile(string id)
        {
            return DeleteAsync(id);
        }
    }
}
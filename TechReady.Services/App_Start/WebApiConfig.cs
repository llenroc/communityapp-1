using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Web.Http;
using TechReady.Services.DataObjects;
using TechReady.Services.Models;
using Microsoft.WindowsAzure.Mobile.Service;

namespace TechReady.Services
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            ConfigOptions options = new ConfigOptions();

            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));



            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<UserProfile, UserProfileDTO>()
                    .ForMember(up => up.InterestedIn,
                        map => map.MapFrom(y => y.InterestedIn));

                cfg.CreateMap<UserProfileDTO, UserProfile>()
                    .ForMember(up => up.InterestedIn,
                        map => map.MapFrom(y => y.InterestedIn));

                cfg.CreateMap<Technology, TechnologyDTO>()
                    .ForMember(technologyDTO => technologyDTO.Id,
                        map => map.MapFrom(technology => MySqlFuncs.LTRIM(MySqlFuncs.StringConvert(technology.Id))));



                cfg.CreateMap<TechnologyDTO, Technology>()
                    .ForMember(technology => technology.Id, map => map.MapFrom(
                        technologyDTO => MySqlFuncs.IntParse(technologyDTO.Id)));

            });


            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            // config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            Database.SetInitializer(new MobileServiceInitializer());
        }
    }

    public class MobileServiceInitializer : DropCreateDatabaseIfModelChanges<MobileServiceContext>
    {
        protected override void Seed(MobileServiceContext context)
        {
            List<TodoItem> todoItems = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "First item", Complete = false },
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Complete = false },
            };

            foreach (TodoItem todoItem in todoItems)
            {
                context.Set<TodoItem>().Add(todoItem);
            }

            base.Seed(context);
        }
    }
}


using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TechReady.Portal.Models
{
    public class TechReadyDbContext : IdentityDbContext<ApplicationUser>
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public TechReadyDbContext() : base("name=DXEvents_SAMPLEContext")
        {
        }


        public static TechReadyDbContext Create()
        {
            return new TechReadyDbContext();
        }

        public System.Data.Entity.DbSet<City> Cities { get; set; }

        public System.Data.Entity.DbSet<Event> Events { get; set; }

        public System.Data.Entity.DbSet<TrackAgenda> TrackAgendas { get; set; }

        public System.Data.Entity.DbSet<EventTrack> EventTracks { get; set; }

        public System.Data.Entity.DbSet<Track> Tracks { get; set; }

        public System.Data.Entity.DbSet<Theme> Themes { get; set; }

        public System.Data.Entity.DbSet<Speaker> Speakers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

        }

        public System.Data.Entity.DbSet<Feedback> Feedbacks { get; set; }

        public System.Data.Entity.DbSet<Session> Sessions { get; set; }

        public System.Data.Entity.DbSet<PrimaryTechnology> PrimaryTechnologies { get; set; }

        public System.Data.Entity.DbSet<SecondaryTechnology> SecondaryTechnologies { get; set; }

        public System.Data.Entity.DbSet<AudienceType> AudienceTypes { get; set; }

        public System.Data.Entity.DbSet<AudienceOrg> AudienceOrgs { get; set; }

        public System.Data.Entity.DbSet<LearningResource> LearningResources { get; set; }

        public System.Data.Entity.DbSet<LearningResourceFeed> LearningResourceFeeds { get; set; }


        public System.Data.Entity.DbSet<LearningResourceType> LearningResourceTypes { get; set; }

        public System.Data.Entity.DbSet<AppUser> AppUsers { get; set; }

        public System.Data.Entity.DbSet<AppFeedback> AppFeedbacks { get; set; }

        public System.Data.Entity.DbSet<Notification> Notifications { get; set; }

        public System.Data.Entity.DbSet<AppUserNotificationAction> AppUserNotificationActions { get; set; }


    }
}

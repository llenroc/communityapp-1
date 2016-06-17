using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Tables;
using TechReady.Services.DataObjects;

namespace TechReady.Services.Models
{

    public class MobileServiceContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to alter your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
        //
        // To enable Entity Framework migrations in the cloud, please ensure that the 
        // service name, set by the 'MS_MobileServiceName' AppSettings in the local 
        // Web.config, is the same as the service name when hosted in Azure.

        private const string connectionStringName = "Name=MS_TableConnectionString";



        public MobileServiceContext() : base(connectionStringName)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Technology> Technologies { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            string schema = ServiceSettingsDictionary.GetSchemaName();
            if (!string.IsNullOrEmpty(schema))
            {
                modelBuilder.HasDefaultSchema(schema);
            }

            modelBuilder.Entity<UserProfile>()
                  .HasMany<Technology>(s => s.InterestedIn)
                  .WithMany(c => c.UserProfiles)
                  .Map(cs =>
                  {
                      cs.MapLeftKey("UserProfileId");
                      cs.MapRightKey("TechnologyId");
                      cs.ToTable("UserTechnologies");
                  });



            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                    "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));
        }
    }

}

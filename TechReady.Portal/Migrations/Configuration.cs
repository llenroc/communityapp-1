using System.Data.Entity.Migrations;
using TechReady.Portal.Models;

namespace TechReady.Portal.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<TechReadyDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "TechReady.Portal.Models.DXEvents_SAMPLEContext";
        }

        protected override void Seed(TechReadyDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}

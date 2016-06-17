using System.Data.Entity.Migrations;

namespace TechReady.Portal.Migrations
{
    public partial class AppUserIntroduced2 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AppUserEvents", newName: "EventAppUsers");
            RenameTable(name: "dbo.AppUserPrimaryTechnologies", newName: "PrimaryTechnologyAppUsers");
            DropPrimaryKey("dbo.EventAppUsers");
            DropPrimaryKey("dbo.PrimaryTechnologyAppUsers");
            AddPrimaryKey("dbo.EventAppUsers", new[] { "Event_EventID", "AppUser_AppUserID" });
            AddPrimaryKey("dbo.PrimaryTechnologyAppUsers", new[] { "PrimaryTechnology_PrimaryTechnologyID", "AppUser_AppUserID" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.PrimaryTechnologyAppUsers");
            DropPrimaryKey("dbo.EventAppUsers");
            AddPrimaryKey("dbo.PrimaryTechnologyAppUsers", new[] { "AppUser_AppUserID", "PrimaryTechnology_PrimaryTechnologyID" });
            AddPrimaryKey("dbo.EventAppUsers", new[] { "AppUser_AppUserID", "Event_EventID" });
            RenameTable(name: "dbo.PrimaryTechnologyAppUsers", newName: "AppUserPrimaryTechnologies");
            RenameTable(name: "dbo.EventAppUsers", newName: "AppUserEvents");
        }
    }
}

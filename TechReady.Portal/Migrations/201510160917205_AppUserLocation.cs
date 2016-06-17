using System.Data.Entity.Migrations;

namespace TechReady.Portal.Migrations
{
    public partial class AppUserLocation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "Town", c => c.String());
            AddColumn("dbo.AppUsers", "Location", c => c.Geography());
            AddColumn("dbo.Cities", "Location", c => c.Geography());
            AlterColumn("dbo.AppUsers", "CityName", c => c.String(maxLength: 40));
            CreateIndex("dbo.AppUsers", "CityName");
            AddForeignKey("dbo.AppUsers", "CityName", "dbo.Cities", "CityName");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppUsers", "CityName", "dbo.Cities");
            DropIndex("dbo.AppUsers", new[] { "CityName" });
            AlterColumn("dbo.AppUsers", "CityName", c => c.String());
            DropColumn("dbo.Cities", "Location");
            DropColumn("dbo.AppUsers", "Location");
            DropColumn("dbo.AppUsers", "Town");
        }
    }
}

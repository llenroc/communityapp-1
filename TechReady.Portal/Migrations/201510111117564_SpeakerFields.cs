using System.Data.Entity.Migrations;

namespace TechReady.Portal.Migrations
{
    public partial class SpeakerFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Speakers", "BlogLink", c => c.String());
            AddColumn("dbo.Speakers", "FacebookLink", c => c.String());
            AddColumn("dbo.Speakers", "LinkedInLink", c => c.String());
            AddColumn("dbo.Speakers", "Location", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Speakers", "Location");
            DropColumn("dbo.Speakers", "LinkedInLink");
            DropColumn("dbo.Speakers", "FacebookLink");
            DropColumn("dbo.Speakers", "BlogLink");
        }
    }
}

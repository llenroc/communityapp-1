using System.Data.Entity.Migrations;

namespace TechReady.Portal.Migrations
{
    public partial class EventGlobal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "IsGlobal", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "IsGlobal");
        }
    }
}

using System.Data.Entity.Migrations;

namespace TechReady.Portal.Migrations
{
    public partial class RemoveNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUserNotificationActions", "Removed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppUserNotificationActions", "Removed");
        }
    }
}

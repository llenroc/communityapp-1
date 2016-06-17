using System.Data.Entity.Migrations;

namespace TechReady.Portal.Migrations
{
    public partial class RequiredNotificationTitle : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Notifications", "NotificationTitle", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notifications", "NotificationTitle", c => c.String());
        }
    }
}

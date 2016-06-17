using System.Data.Entity.Migrations;

namespace TechReady.Portal.Migrations
{
    public partial class Notifications : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppUserNotificationActions",
                c => new
                    {
                        AppUserNotificationActionID = c.Int(nullable: false, identity: true),
                        Read = c.Boolean(nullable: false),
                        NotificationID = c.Int(nullable: false),
                        AppUserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AppUserNotificationActionID)
                .ForeignKey("dbo.AppUsers", t => t.AppUserID)
                .ForeignKey("dbo.Notifications", t => t.NotificationID)
                .Index(t => t.NotificationID)
                .Index(t => t.AppUserID);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        NotificationID = c.Int(nullable: false, identity: true),
                        TypeOfNotification = c.Int(nullable: false),
                        ResourceId = c.Int(),
                        ActionLink = c.String(),
                        NotificationTitle = c.String(),
                        NotificationMessage = c.String(),
                        PushDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.NotificationID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppUserNotificationActions", "NotificationID", "dbo.Notifications");
            DropForeignKey("dbo.AppUserNotificationActions", "AppUserID", "dbo.AppUsers");
            DropIndex("dbo.AppUserNotificationActions", new[] { "AppUserID" });
            DropIndex("dbo.AppUserNotificationActions", new[] { "NotificationID" });
            DropTable("dbo.Notifications");
            DropTable("dbo.AppUserNotificationActions");
        }
    }
}

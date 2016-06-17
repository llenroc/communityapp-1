using System.Data.Entity.Migrations;

namespace TechReady.Portal.Migrations
{
    public partial class Notifications2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotificationAudienceTypes",
                c => new
                    {
                        Notification_NotificationID = c.Int(nullable: false),
                        AudienceType_AudienceTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Notification_NotificationID, t.AudienceType_AudienceTypeID })
                .ForeignKey("dbo.Notifications", t => t.Notification_NotificationID, cascadeDelete: true)
                .ForeignKey("dbo.AudienceTypes", t => t.AudienceType_AudienceTypeID, cascadeDelete: true)
                .Index(t => t.Notification_NotificationID)
                .Index(t => t.AudienceType_AudienceTypeID);
            
            CreateTable(
                "dbo.PrimaryTechnologyNotifications",
                c => new
                    {
                        PrimaryTechnology_PrimaryTechnologyID = c.Int(nullable: false),
                        Notification_NotificationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PrimaryTechnology_PrimaryTechnologyID, t.Notification_NotificationID })
                .ForeignKey("dbo.PrimaryTechnologies", t => t.PrimaryTechnology_PrimaryTechnologyID, cascadeDelete: true)
                .ForeignKey("dbo.Notifications", t => t.Notification_NotificationID, cascadeDelete: true)
                .Index(t => t.PrimaryTechnology_PrimaryTechnologyID)
                .Index(t => t.Notification_NotificationID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PrimaryTechnologyNotifications", "Notification_NotificationID", "dbo.Notifications");
            DropForeignKey("dbo.PrimaryTechnologyNotifications", "PrimaryTechnology_PrimaryTechnologyID", "dbo.PrimaryTechnologies");
            DropForeignKey("dbo.NotificationAudienceTypes", "AudienceType_AudienceTypeID", "dbo.AudienceTypes");
            DropForeignKey("dbo.NotificationAudienceTypes", "Notification_NotificationID", "dbo.Notifications");
            DropIndex("dbo.PrimaryTechnologyNotifications", new[] { "Notification_NotificationID" });
            DropIndex("dbo.PrimaryTechnologyNotifications", new[] { "PrimaryTechnology_PrimaryTechnologyID" });
            DropIndex("dbo.NotificationAudienceTypes", new[] { "AudienceType_AudienceTypeID" });
            DropIndex("dbo.NotificationAudienceTypes", new[] { "Notification_NotificationID" });
            DropTable("dbo.PrimaryTechnologyNotifications");
            DropTable("dbo.NotificationAudienceTypes");
        }
    }
}

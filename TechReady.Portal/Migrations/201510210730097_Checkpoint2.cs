using System.Data.Entity.Migrations;

namespace TechReady.Portal.Migrations
{
    public partial class Checkpoint2 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PrimaryTechnologyNotifications", newName: "NotificationPrimaryTechnologies");
            DropForeignKey("dbo.LearningResources", "AudienceTypeID", "dbo.AudienceTypes");
            DropForeignKey("dbo.LearningResourceFeeds", "AudienceTypeID", "dbo.AudienceTypes");
            DropIndex("dbo.LearningResources", new[] { "AudienceTypeID" });
            DropIndex("dbo.LearningResourceFeeds", new[] { "AudienceTypeID" });
            DropPrimaryKey("dbo.NotificationPrimaryTechnologies");
            CreateTable(
                "dbo.LearningResourceFeedAudienceTypes",
                c => new
                    {
                        LearningResourceFeed_LearningResourceFeedID = c.Int(nullable: false),
                        AudienceType_AudienceTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LearningResourceFeed_LearningResourceFeedID, t.AudienceType_AudienceTypeID })
                .ForeignKey("dbo.LearningResourceFeeds", t => t.LearningResourceFeed_LearningResourceFeedID, cascadeDelete: true)
                .ForeignKey("dbo.AudienceTypes", t => t.AudienceType_AudienceTypeID, cascadeDelete: true)
                .Index(t => t.LearningResourceFeed_LearningResourceFeedID)
                .Index(t => t.AudienceType_AudienceTypeID);
            
            CreateTable(
                "dbo.LearningResourceAudienceTypes",
                c => new
                    {
                        LearningResource_LearningResourceID = c.Int(nullable: false),
                        AudienceType_AudienceTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LearningResource_LearningResourceID, t.AudienceType_AudienceTypeID })
                .ForeignKey("dbo.LearningResources", t => t.LearningResource_LearningResourceID, cascadeDelete: true)
                .ForeignKey("dbo.AudienceTypes", t => t.AudienceType_AudienceTypeID, cascadeDelete: true)
                .Index(t => t.LearningResource_LearningResourceID)
                .Index(t => t.AudienceType_AudienceTypeID);
            
            AlterColumn("dbo.Events", "EventAbstract", c => c.String());
            AlterColumn("dbo.Tracks", "TrackAbstract", c => c.String(nullable: false));
            AlterColumn("dbo.Sessions", "Abstract", c => c.String());
            AlterColumn("dbo.Speakers", "Profile", c => c.String(nullable: false));
            AddPrimaryKey("dbo.NotificationPrimaryTechnologies", new[] { "Notification_NotificationID", "PrimaryTechnology_PrimaryTechnologyID" });
            DropColumn("dbo.LearningResources", "AudienceTypeID");
            DropColumn("dbo.LearningResourceFeeds", "AudienceTypeID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LearningResourceFeeds", "AudienceTypeID", c => c.Int());
            AddColumn("dbo.LearningResources", "AudienceTypeID", c => c.Int());
            DropForeignKey("dbo.LearningResourceAudienceTypes", "AudienceType_AudienceTypeID", "dbo.AudienceTypes");
            DropForeignKey("dbo.LearningResourceAudienceTypes", "LearningResource_LearningResourceID", "dbo.LearningResources");
            DropForeignKey("dbo.LearningResourceFeedAudienceTypes", "AudienceType_AudienceTypeID", "dbo.AudienceTypes");
            DropForeignKey("dbo.LearningResourceFeedAudienceTypes", "LearningResourceFeed_LearningResourceFeedID", "dbo.LearningResourceFeeds");
            DropIndex("dbo.LearningResourceAudienceTypes", new[] { "AudienceType_AudienceTypeID" });
            DropIndex("dbo.LearningResourceAudienceTypes", new[] { "LearningResource_LearningResourceID" });
            DropIndex("dbo.LearningResourceFeedAudienceTypes", new[] { "AudienceType_AudienceTypeID" });
            DropIndex("dbo.LearningResourceFeedAudienceTypes", new[] { "LearningResourceFeed_LearningResourceFeedID" });
            DropPrimaryKey("dbo.NotificationPrimaryTechnologies");
            AlterColumn("dbo.Speakers", "Profile", c => c.String(nullable: false, maxLength: 2000));
            AlterColumn("dbo.Sessions", "Abstract", c => c.String(maxLength: 250));
            AlterColumn("dbo.Tracks", "TrackAbstract", c => c.String(nullable: false, maxLength: 600));
            AlterColumn("dbo.Events", "EventAbstract", c => c.String(maxLength: 1000));
            DropTable("dbo.LearningResourceAudienceTypes");
            DropTable("dbo.LearningResourceFeedAudienceTypes");
            AddPrimaryKey("dbo.NotificationPrimaryTechnologies", new[] { "PrimaryTechnology_PrimaryTechnologyID", "Notification_NotificationID" });
            CreateIndex("dbo.LearningResourceFeeds", "AudienceTypeID");
            CreateIndex("dbo.LearningResources", "AudienceTypeID");
            AddForeignKey("dbo.LearningResourceFeeds", "AudienceTypeID", "dbo.AudienceTypes", "AudienceTypeID");
            AddForeignKey("dbo.LearningResources", "AudienceTypeID", "dbo.AudienceTypes", "AudienceTypeID");
            RenameTable(name: "dbo.NotificationPrimaryTechnologies", newName: "PrimaryTechnologyNotifications");
        }
    }
}

using System.Data.Entity.Migrations;

namespace TechReady.Portal.Migrations
{
    public partial class AddLearningResources : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LearningResourceFeeds",
                c => new
                    {
                        LearningResourceFeedID = c.Int(nullable: false, identity: true),
                        PrimaryTechnologyID = c.Int(),
                        AudienceTypeID = c.Int(),
                        LearningResourceTypeID = c.Int(nullable: false),
                        RSSLink = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.LearningResourceFeedID)
                .ForeignKey("dbo.AudienceTypes", t => t.AudienceTypeID)
                .ForeignKey("dbo.LearningResourceTypes", t => t.LearningResourceTypeID)
                .ForeignKey("dbo.PrimaryTechnologies", t => t.PrimaryTechnologyID)
                .Index(t => t.PrimaryTechnologyID)
                .Index(t => t.AudienceTypeID)
                .Index(t => t.LearningResourceTypeID);
            
            CreateTable(
                "dbo.LearningResourceTypes",
                c => new
                    {
                        LearningResourceTypeID = c.Int(nullable: false, identity: true),
                        LearningResourceTypeName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.LearningResourceTypeID);
            
            CreateTable(
                "dbo.LearningResources",
                c => new
                    {
                        LearningResourceID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                        ThumbnailURL = c.String(),
                        LearningResourceTypeID = c.Int(nullable: false),
                        ContentURL = c.String(nullable: false),
                        PublicationTime = c.DateTime(nullable: false),
                        PrimaryTechnologyID = c.Int(),
                        AudienceTypeID = c.Int(),
                    })
                .PrimaryKey(t => t.LearningResourceID)
                .ForeignKey("dbo.AudienceTypes", t => t.AudienceTypeID)
                .ForeignKey("dbo.LearningResourceTypes", t => t.LearningResourceTypeID)
                .ForeignKey("dbo.PrimaryTechnologies", t => t.PrimaryTechnologyID)
                .Index(t => t.LearningResourceTypeID)
                .Index(t => t.PrimaryTechnologyID)
                .Index(t => t.AudienceTypeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LearningResources", "PrimaryTechnologyID", "dbo.PrimaryTechnologies");
            DropForeignKey("dbo.LearningResources", "LearningResourceTypeID", "dbo.LearningResourceTypes");
            DropForeignKey("dbo.LearningResources", "AudienceTypeID", "dbo.AudienceTypes");
            DropForeignKey("dbo.LearningResourceFeeds", "PrimaryTechnologyID", "dbo.PrimaryTechnologies");
            DropForeignKey("dbo.LearningResourceFeeds", "LearningResourceTypeID", "dbo.LearningResourceTypes");
            DropForeignKey("dbo.LearningResourceFeeds", "AudienceTypeID", "dbo.AudienceTypes");
            DropIndex("dbo.LearningResources", new[] { "AudienceTypeID" });
            DropIndex("dbo.LearningResources", new[] { "PrimaryTechnologyID" });
            DropIndex("dbo.LearningResources", new[] { "LearningResourceTypeID" });
            DropIndex("dbo.LearningResourceFeeds", new[] { "LearningResourceTypeID" });
            DropIndex("dbo.LearningResourceFeeds", new[] { "AudienceTypeID" });
            DropIndex("dbo.LearningResourceFeeds", new[] { "PrimaryTechnologyID" });
            DropTable("dbo.LearningResources");
            DropTable("dbo.LearningResourceTypes");
            DropTable("dbo.LearningResourceFeeds");
        }
    }
}

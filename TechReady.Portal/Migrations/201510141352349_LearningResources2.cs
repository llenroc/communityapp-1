using System.Data.Entity.Migrations;

namespace TechReady.Portal.Migrations
{
    public partial class LearningResources2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.LearningResourceFeeds", new[] { "LearningResourceTypeID" });
            DropIndex("dbo.LearningResources", new[] { "LearningResourceTypeID" });
            AlterColumn("dbo.LearningResourceFeeds", "LearningResourceTypeID", c => c.Int());
            AlterColumn("dbo.LearningResources", "LearningResourceTypeID", c => c.Int());
            CreateIndex("dbo.LearningResourceFeeds", "LearningResourceTypeID");
            CreateIndex("dbo.LearningResources", "LearningResourceTypeID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.LearningResources", new[] { "LearningResourceTypeID" });
            DropIndex("dbo.LearningResourceFeeds", new[] { "LearningResourceTypeID" });
            AlterColumn("dbo.LearningResources", "LearningResourceTypeID", c => c.Int(nullable: false));
            AlterColumn("dbo.LearningResourceFeeds", "LearningResourceTypeID", c => c.Int(nullable: false));
            CreateIndex("dbo.LearningResources", "LearningResourceTypeID");
            CreateIndex("dbo.LearningResourceFeeds", "LearningResourceTypeID");
        }
    }
}

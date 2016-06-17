using System.Data.Entity.Migrations;

namespace TechReady.Portal.Migrations
{
    public partial class AppFeedback : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppFeedbacks",
                c => new
                    {
                        AppFeedbackID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        FeedbackType = c.String(),
                        FeedbackText = c.String(),
                        AppUserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AppFeedbackID)
                .ForeignKey("dbo.AppUsers", t => t.AppUserID)
                .Index(t => t.AppUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppFeedbacks", "AppUserID", "dbo.AppUsers");
            DropIndex("dbo.AppFeedbacks", new[] { "AppUserID" });
            DropTable("dbo.AppFeedbacks");
        }
    }
}

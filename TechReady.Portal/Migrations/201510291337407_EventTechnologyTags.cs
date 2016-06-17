using System.Data.Entity.Migrations;

namespace TechReady.Portal.Migrations
{
    public partial class EventTechnologyTags : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventPrimaryTechnologies",
                c => new
                    {
                        Event_EventID = c.Int(nullable: false),
                        PrimaryTechnology_PrimaryTechnologyID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Event_EventID, t.PrimaryTechnology_PrimaryTechnologyID })
                .ForeignKey("dbo.Events", t => t.Event_EventID, cascadeDelete: true)
                .ForeignKey("dbo.PrimaryTechnologies", t => t.PrimaryTechnology_PrimaryTechnologyID, cascadeDelete: true)
                .Index(t => t.Event_EventID)
                .Index(t => t.PrimaryTechnology_PrimaryTechnologyID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventPrimaryTechnologies", "PrimaryTechnology_PrimaryTechnologyID", "dbo.PrimaryTechnologies");
            DropForeignKey("dbo.EventPrimaryTechnologies", "Event_EventID", "dbo.Events");
            DropIndex("dbo.EventPrimaryTechnologies", new[] { "PrimaryTechnology_PrimaryTechnologyID" });
            DropIndex("dbo.EventPrimaryTechnologies", new[] { "Event_EventID" });
            DropTable("dbo.EventPrimaryTechnologies");
        }
    }
}

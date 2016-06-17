using System.Data.Entity.Migrations;

namespace TechReady.Portal.Migrations
{
    public partial class EventAudienceTypeTags : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.EventPrimaryTechnologies", newName: "PrimaryTechnologyEvents");
            DropPrimaryKey("dbo.PrimaryTechnologyEvents");
            CreateTable(
                "dbo.EventAudienceTypes",
                c => new
                    {
                        Event_EventID = c.Int(nullable: false),
                        AudienceType_AudienceTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Event_EventID, t.AudienceType_AudienceTypeID })
                .ForeignKey("dbo.Events", t => t.Event_EventID, cascadeDelete: true)
                .ForeignKey("dbo.AudienceTypes", t => t.AudienceType_AudienceTypeID, cascadeDelete: true)
                .Index(t => t.Event_EventID)
                .Index(t => t.AudienceType_AudienceTypeID);
            
            AddPrimaryKey("dbo.PrimaryTechnologyEvents", new[] { "PrimaryTechnology_PrimaryTechnologyID", "Event_EventID" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventAudienceTypes", "AudienceType_AudienceTypeID", "dbo.AudienceTypes");
            DropForeignKey("dbo.EventAudienceTypes", "Event_EventID", "dbo.Events");
            DropIndex("dbo.EventAudienceTypes", new[] { "AudienceType_AudienceTypeID" });
            DropIndex("dbo.EventAudienceTypes", new[] { "Event_EventID" });
            DropPrimaryKey("dbo.PrimaryTechnologyEvents");
            DropTable("dbo.EventAudienceTypes");
            AddPrimaryKey("dbo.PrimaryTechnologyEvents", new[] { "Event_EventID", "PrimaryTechnology_PrimaryTechnologyID" });
            RenameTable(name: "dbo.PrimaryTechnologyEvents", newName: "EventPrimaryTechnologies");
        }
    }
}

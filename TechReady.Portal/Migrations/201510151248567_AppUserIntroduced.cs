using System.Data.Entity.Migrations;

namespace TechReady.Portal.Migrations
{
    public partial class AppUserIntroduced : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppUsers",
                c => new
                    {
                        AppUserID = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        Email = c.String(),
                        CityName = c.String(),
                        AudienceOrgID = c.Int(nullable: false),
                        AuthProviderUserId = c.String(),
                        AuthProviderName = c.String(),
                        DevicePlatform = c.String(),
                        RegistrationDateTime = c.DateTime(),
                        LastAccessTime = c.DateTime(),
                        PushEnabled = c.Boolean(nullable: false),
                        DeviceId = c.String(),
                        PushId = c.String(),
                    })
                .PrimaryKey(t => t.AppUserID)
                .ForeignKey("dbo.AudienceOrgs", t => t.AudienceOrgID)
                .Index(t => t.AudienceOrgID);
            
            CreateTable(
                "dbo.AppUserEvents",
                c => new
                    {
                        AppUser_AppUserID = c.Int(nullable: false),
                        Event_EventID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AppUser_AppUserID, t.Event_EventID })
                .ForeignKey("dbo.AppUsers", t => t.AppUser_AppUserID, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.Event_EventID, cascadeDelete: true)
                .Index(t => t.AppUser_AppUserID)
                .Index(t => t.Event_EventID);
            
            CreateTable(
                "dbo.SpeakerAppUsers",
                c => new
                    {
                        Speaker_SpeakerID = c.Int(nullable: false),
                        AppUser_AppUserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Speaker_SpeakerID, t.AppUser_AppUserID })
                .ForeignKey("dbo.Speakers", t => t.Speaker_SpeakerID, cascadeDelete: true)
                .ForeignKey("dbo.AppUsers", t => t.AppUser_AppUserID, cascadeDelete: true)
                .Index(t => t.Speaker_SpeakerID)
                .Index(t => t.AppUser_AppUserID);
            
            CreateTable(
                "dbo.AppUserPrimaryTechnologies",
                c => new
                    {
                        AppUser_AppUserID = c.Int(nullable: false),
                        PrimaryTechnology_PrimaryTechnologyID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AppUser_AppUserID, t.PrimaryTechnology_PrimaryTechnologyID })
                .ForeignKey("dbo.AppUsers", t => t.AppUser_AppUserID, cascadeDelete: true)
                .ForeignKey("dbo.PrimaryTechnologies", t => t.PrimaryTechnology_PrimaryTechnologyID, cascadeDelete: true)
                .Index(t => t.AppUser_AppUserID)
                .Index(t => t.PrimaryTechnology_PrimaryTechnologyID);
            
            CreateTable(
                "dbo.LearningResourceAppUsers",
                c => new
                    {
                        LearningResource_LearningResourceID = c.Int(nullable: false),
                        AppUser_AppUserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LearningResource_LearningResourceID, t.AppUser_AppUserID })
                .ForeignKey("dbo.LearningResources", t => t.LearningResource_LearningResourceID, cascadeDelete: true)
                .ForeignKey("dbo.AppUsers", t => t.AppUser_AppUserID, cascadeDelete: true)
                .Index(t => t.LearningResource_LearningResourceID)
                .Index(t => t.AppUser_AppUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LearningResourceAppUsers", "AppUser_AppUserID", "dbo.AppUsers");
            DropForeignKey("dbo.LearningResourceAppUsers", "LearningResource_LearningResourceID", "dbo.LearningResources");
            DropForeignKey("dbo.AppUserPrimaryTechnologies", "PrimaryTechnology_PrimaryTechnologyID", "dbo.PrimaryTechnologies");
            DropForeignKey("dbo.AppUserPrimaryTechnologies", "AppUser_AppUserID", "dbo.AppUsers");
            DropForeignKey("dbo.SpeakerAppUsers", "AppUser_AppUserID", "dbo.AppUsers");
            DropForeignKey("dbo.SpeakerAppUsers", "Speaker_SpeakerID", "dbo.Speakers");
            DropForeignKey("dbo.AppUserEvents", "Event_EventID", "dbo.Events");
            DropForeignKey("dbo.AppUserEvents", "AppUser_AppUserID", "dbo.AppUsers");
            DropForeignKey("dbo.AppUsers", "AudienceOrgID", "dbo.AudienceOrgs");
            DropIndex("dbo.LearningResourceAppUsers", new[] { "AppUser_AppUserID" });
            DropIndex("dbo.LearningResourceAppUsers", new[] { "LearningResource_LearningResourceID" });
            DropIndex("dbo.AppUserPrimaryTechnologies", new[] { "PrimaryTechnology_PrimaryTechnologyID" });
            DropIndex("dbo.AppUserPrimaryTechnologies", new[] { "AppUser_AppUserID" });
            DropIndex("dbo.SpeakerAppUsers", new[] { "AppUser_AppUserID" });
            DropIndex("dbo.SpeakerAppUsers", new[] { "Speaker_SpeakerID" });
            DropIndex("dbo.AppUserEvents", new[] { "Event_EventID" });
            DropIndex("dbo.AppUserEvents", new[] { "AppUser_AppUserID" });
            DropIndex("dbo.AppUsers", new[] { "AudienceOrgID" });
            DropTable("dbo.LearningResourceAppUsers");
            DropTable("dbo.AppUserPrimaryTechnologies");
            DropTable("dbo.SpeakerAppUsers");
            DropTable("dbo.AppUserEvents");
            DropTable("dbo.AppUsers");
        }
    }
}

namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersComments : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ApplicationUserMedals", newName: "MedalApplicationUsers");
            DropPrimaryKey("dbo.MedalApplicationUsers");
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false),
                        CreationTime = c.String(nullable: false),
                        OwnerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.CommentApplicationUsers",
                c => new
                    {
                        Comment_Id = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Comment_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.Comments", t => t.Comment_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Comment_Id)
                .Index(t => t.ApplicationUser_Id);
            
            AddPrimaryKey("dbo.MedalApplicationUsers", new[] { "Medal_Id", "ApplicationUser_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CommentApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CommentApplicationUsers", "Comment_Id", "dbo.Comments");
            DropIndex("dbo.CommentApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.CommentApplicationUsers", new[] { "Comment_Id" });
            DropIndex("dbo.Comments", new[] { "OwnerId" });
            DropPrimaryKey("dbo.MedalApplicationUsers");
            DropTable("dbo.CommentApplicationUsers");
            DropTable("dbo.Comments");
            AddPrimaryKey("dbo.MedalApplicationUsers", new[] { "ApplicationUser_Id", "Medal_Id" });
            RenameTable(name: "dbo.MedalApplicationUsers", newName: "ApplicationUserMedals");
        }
    }
}

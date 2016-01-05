namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Postcard : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Postcards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ImageUrl = c.String(nullable: false),
                        ThumbnailUrl = c.String(nullable: false),
                        AverageRating = c.Int(nullable: false),
                        CreationTime = c.String(nullable: false),
                        OwnerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId)
                .Index(t => t.OwnerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Postcards", "OwnerId", "dbo.AspNetUsers");
            DropIndex("dbo.Postcards", new[] { "OwnerId" });
            DropTable("dbo.Postcards");
        }
    }
}

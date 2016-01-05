namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserRatingsPostcardRatings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.Int(nullable: false),
                        RaterId = c.String(maxLength: 128),
                        RelatedPostcardId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.RaterId)
                .ForeignKey("dbo.Postcards", t => t.RelatedPostcardId)
                .Index(t => t.RaterId)
                .Index(t => t.RelatedPostcardId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ratings", "RelatedPostcardId", "dbo.Postcards");
            DropForeignKey("dbo.Ratings", "RaterId", "dbo.AspNetUsers");
            DropIndex("dbo.Ratings", new[] { "RelatedPostcardId" });
            DropIndex("dbo.Ratings", new[] { "RaterId" });
            DropTable("dbo.Ratings");
        }
    }
}

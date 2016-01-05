namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostcardComments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "RelatedPostcardId", c => c.Int());
            CreateIndex("dbo.Comments", "RelatedPostcardId");
            AddForeignKey("dbo.Comments", "RelatedPostcardId", "dbo.Postcards", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "RelatedPostcardId", "dbo.Postcards");
            DropIndex("dbo.Comments", new[] { "RelatedPostcardId" });
            DropColumn("dbo.Comments", "RelatedPostcardId");
        }
    }
}

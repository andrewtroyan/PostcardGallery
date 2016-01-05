namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HashTagsPostcards : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HashTags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HashTagPostcards",
                c => new
                    {
                        HashTag_Id = c.Int(nullable: false),
                        Postcard_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.HashTag_Id, t.Postcard_Id })
                .ForeignKey("dbo.HashTags", t => t.HashTag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Postcards", t => t.Postcard_Id, cascadeDelete: true)
                .Index(t => t.HashTag_Id)
                .Index(t => t.Postcard_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HashTagPostcards", "Postcard_Id", "dbo.Postcards");
            DropForeignKey("dbo.HashTagPostcards", "HashTag_Id", "dbo.HashTags");
            DropIndex("dbo.HashTagPostcards", new[] { "Postcard_Id" });
            DropIndex("dbo.HashTagPostcards", new[] { "HashTag_Id" });
            DropTable("dbo.HashTagPostcards");
            DropTable("dbo.HashTags");
        }
    }
}

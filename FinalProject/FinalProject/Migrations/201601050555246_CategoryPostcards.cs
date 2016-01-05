namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryPostcards : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Postcards", "CategoryId", c => c.Int());
            CreateIndex("dbo.Postcards", "CategoryId");
            AddForeignKey("dbo.Postcards", "CategoryId", "dbo.Categories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Postcards", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Postcards", new[] { "CategoryId" });
            DropColumn("dbo.Postcards", "CategoryId");
            DropTable("dbo.Categories");
        }
    }
}

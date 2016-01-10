namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Postcards : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Postcards", "ImagePath", c => c.String(nullable: false));
            AddColumn("dbo.Postcards", "JsonPath", c => c.String(nullable: false));
            AddColumn("dbo.Postcards", "TemplateId", c => c.Int());
            CreateIndex("dbo.Postcards", "TemplateId");
            AddForeignKey("dbo.Postcards", "TemplateId", "dbo.Templates", "Id");
            DropColumn("dbo.Postcards", "ImageUrl");
            DropColumn("dbo.Postcards", "ThumbnailUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Postcards", "ThumbnailUrl", c => c.String(nullable: false));
            AddColumn("dbo.Postcards", "ImageUrl", c => c.String(nullable: false));
            DropForeignKey("dbo.Postcards", "TemplateId", "dbo.Templates");
            DropIndex("dbo.Postcards", new[] { "TemplateId" });
            DropColumn("dbo.Postcards", "TemplateId");
            DropColumn("dbo.Postcards", "JsonPath");
            DropColumn("dbo.Postcards", "ImagePath");
        }
    }
}

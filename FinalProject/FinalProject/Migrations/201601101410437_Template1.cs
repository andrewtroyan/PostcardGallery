namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Template1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Templates", "JsonPath", c => c.String(nullable: false));
            DropColumn("dbo.Templates", "JsonData");
            DropColumn("dbo.Templates", "TemplateUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Templates", "TemplateUrl", c => c.String(nullable: false));
            AddColumn("dbo.Templates", "JsonData", c => c.String(nullable: false));
            DropColumn("dbo.Templates", "JsonPath");
        }
    }
}

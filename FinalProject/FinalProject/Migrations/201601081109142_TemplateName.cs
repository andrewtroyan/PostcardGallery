namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TemplateName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Templates", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Templates", "Name");
        }
    }
}

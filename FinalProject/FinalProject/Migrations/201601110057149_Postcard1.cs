namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Postcard1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Postcards", "ImageUrl", c => c.String(nullable: false));
            DropColumn("dbo.Postcards", "ImagePath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Postcards", "ImagePath", c => c.String(nullable: false));
            DropColumn("dbo.Postcards", "ImageUrl");
        }
    }
}

namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Postcard2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Postcards", "ImagePath", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Postcards", "ImagePath");
        }
    }
}

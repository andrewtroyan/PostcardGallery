namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostcardsCreationTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Postcards", "CreationTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Postcards", "CreationTime", c => c.String(nullable: false));
        }
    }
}

namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostcardAverageRating1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Postcards", "AverageRating", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Postcards", "AverageRating");
        }
    }
}

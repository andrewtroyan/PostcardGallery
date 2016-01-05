namespace FinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostcardAverageRating : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Postcards", "AverageRating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Postcards", "AverageRating", c => c.Int(nullable: false));
        }
    }
}

namespace MvcAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEnumTypes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Densities", "County", c => c.Int(nullable: false));
            AlterColumn("dbo.Jobs", "Category", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Jobs", "Category", c => c.String());
            AlterColumn("dbo.Densities", "County", c => c.String());
        }
    }
}

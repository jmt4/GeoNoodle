namespace MvcAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DensityValueToInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Densities", "Value", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Densities", "Value", c => c.Double(nullable: false));
        }
    }
}

namespace MvcAuth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DensityJobCountICollection : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Densities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        JobID = c.Int(nullable: false),
                        County = c.String(),
                        Value = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Jobs", t => t.JobID, cascadeDelete: true)
                .Index(t => t.JobID);
            
            CreateTable(
                "dbo.JobCounts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        JobID = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Jobs", t => t.JobID, cascadeDelete: true)
                .Index(t => t.JobID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobCounts", "JobID", "dbo.Jobs");
            DropForeignKey("dbo.Densities", "JobID", "dbo.Jobs");
            DropIndex("dbo.JobCounts", new[] { "JobID" });
            DropIndex("dbo.Densities", new[] { "JobID" });
            DropTable("dbo.JobCounts");
            DropTable("dbo.Densities");
        }
    }
}

namespace UrlCrawlerBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CrawlerConfigs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Route = c.String(),
                        Path = c.String(),
                        LinkSelector = c.String(),
                        TitleSelector = c.String(),
                        DescriptionSelector = c.String(),
                        ContentSelector = c.String(),
                        RemovalSelector = c.String(),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CrawlerConfigs", "CategoryId", "dbo.Categories");
            DropIndex("dbo.CrawlerConfigs", new[] { "CategoryId" });
            DropTable("dbo.CrawlerConfigs");
            DropTable("dbo.Categories");
        }
    }
}

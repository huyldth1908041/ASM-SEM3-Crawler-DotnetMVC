namespace CrawlerApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitCrawlerApiContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Content = c.String(),
                        Source = c.String(),
                        Link = c.String(),
                        ImgUrls = c.String(),
                        UpdatedAt = c.DateTime(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
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
            
            AddColumn("dbo.AspNetRoles", "Description", c => c.String());
            AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
            AddColumn("dbo.AspNetUsers", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CrawlerConfigs", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Articles", "CategoryId", "dbo.Categories");
            DropIndex("dbo.CrawlerConfigs", new[] { "CategoryId" });
            DropIndex("dbo.Articles", new[] { "CategoryId" });
            DropColumn("dbo.AspNetUsers", "Discriminator");
            DropColumn("dbo.AspNetUsers", "Address");
            DropColumn("dbo.AspNetRoles", "Discriminator");
            DropColumn("dbo.AspNetRoles", "Description");
            DropTable("dbo.CrawlerConfigs");
            DropTable("dbo.Categories");
            DropTable("dbo.Articles");
        }
    }
}

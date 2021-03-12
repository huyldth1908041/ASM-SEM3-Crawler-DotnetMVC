namespace UrlCrawlerBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddArticle : DbMigration
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Articles", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Articles", new[] { "CategoryId" });
            DropTable("dbo.Articles");
        }
    }
}

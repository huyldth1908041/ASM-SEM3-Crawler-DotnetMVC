namespace CrawlerApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitCrawlerApiContext3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRecentViewArticles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ViewedAt = c.DateTime(nullable: false),
                        Article_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.Article_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Article_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.UserSavedArticles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SavedAt = c.DateTime(nullable: false),
                        Article_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.Article_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Article_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRecentViewArticles", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserSavedArticles", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserSavedArticles", "Article_Id", "dbo.Articles");
            DropForeignKey("dbo.UserRecentViewArticles", "Article_Id", "dbo.Articles");
            DropIndex("dbo.UserSavedArticles", new[] { "User_Id" });
            DropIndex("dbo.UserSavedArticles", new[] { "Article_Id" });
            DropIndex("dbo.UserRecentViewArticles", new[] { "User_Id" });
            DropIndex("dbo.UserRecentViewArticles", new[] { "Article_Id" });
            DropTable("dbo.UserSavedArticles");
            DropTable("dbo.UserRecentViewArticles");
        }
    }
}

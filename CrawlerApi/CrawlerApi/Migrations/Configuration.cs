namespace CrawlerApi.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CrawlerApi.Models.OwinApiContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CrawlerApi.Models.OwinApiContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Categories.AddOrUpdate(x => x.Id,
                new Models.Category()
                {
                    Id = 1,
                    Name = "Thời sự"
                },
                new Models.Category()
                {
                    Id = 2,
                    Name = "Giải trí"
                }
                );

            context.CrawlerConfigs.AddOrUpdate(x => x.Id,
                new Models.CrawlerConfig()
                {
                    Id = 1,
                    Route = "https://vnexpress.net",
                    Path = "/thoi-su",
                    LinkSelector = "h3.title-news>a",
                    TitleSelector = "h1.title-detail",
                    DescriptionSelector = "p.description",
                    ContentSelector = "article.fck_detail",
                    RemovalSelector = "div.list_link, ul.list-news.hidden",
                    CategoryId = 1
                },
                new Models.CrawlerConfig()
                {
                    Id = 1,
                    Route = "https://vnexpress.net",
                    Path = "/giai-tri",
                    LinkSelector = "h3.title-news>a",
                    TitleSelector = "h1.title-detail",
                    DescriptionSelector = "p.description",
                    ContentSelector = "article.fck_detail",
                    RemovalSelector = "div.list_link, ul.list-news.hidden",
                    CategoryId = 2
                }
                );
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlCrawlerBot.Models
{
    public class UrlCrawlerBotDbContext: DbContext
    {

        public UrlCrawlerBotDbContext() : base("urlCrawlerContext")
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CrawlerConfig> CrawlerConfigs { get; set; }
        public DbSet<Article> Articles { get; set; }
    }
}

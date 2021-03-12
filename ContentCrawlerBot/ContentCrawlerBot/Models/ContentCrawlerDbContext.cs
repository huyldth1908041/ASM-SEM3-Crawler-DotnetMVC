using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentCrawlerBot.Models
{
    class ContentCrawlerDbContext : DbContext
    {
        public ContentCrawlerDbContext() : base("contentCrawlerContext")
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CrawlerConfig> CrawlerConfigs { get; set; }
        public DbSet<Article> Articles { get; set; }
    }
}

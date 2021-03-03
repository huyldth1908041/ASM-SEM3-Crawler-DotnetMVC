using System.Collections.Generic;

namespace CrawlerApi.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //navigation property
        public virtual ICollection<CrawlerConfig> CrawlerConfigs { get; set; }

        public virtual ICollection<Article> Articles { get; set; }

    }
}
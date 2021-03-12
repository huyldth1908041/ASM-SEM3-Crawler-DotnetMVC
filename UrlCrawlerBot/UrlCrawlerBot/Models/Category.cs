using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlCrawlerBot.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //navigation property
        public virtual ICollection<CrawlerConfig> CrawlerConfigs { get; set; }
    }
}

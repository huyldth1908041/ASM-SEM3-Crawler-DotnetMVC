using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlCrawlerBot.Models
{
    public class CrawlerConfig
    {
        public int Id { get; set; }
        public string Route { get; set; }
        public string Path { get; set; }
        public string LinkSelector { get; set; }
        public string TitleSelector { get; set; }
        public string DescriptionSelector { get; set; }
        public string ContentSelector { get; set; }
        public string RemovalSelector { get; set; }

        public int CategoryId { get; set; }
        //navigation propery
        public virtual Category Category { get; set; }
    }
}

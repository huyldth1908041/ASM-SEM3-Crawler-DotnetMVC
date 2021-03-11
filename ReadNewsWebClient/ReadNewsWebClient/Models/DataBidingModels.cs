using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadNewsWebClient.Models
{
    public class ArticleDataBindingModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Source { get; set; }
        public string Link { get; set; }
        public string ImgUrls { get; set; }
        public int Status { get; set; }

        //foreing key
        public int CategoryId { get; set; }
    }
}
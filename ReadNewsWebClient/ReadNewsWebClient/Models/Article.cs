using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadNewsWebClient.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Source { get; set; }
        public string Link { get; set; }
        public string ImgUrls { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Status { get; set; }

        //foreing key
        public int CategoryId { get; set; }

        public string GetImageUrl()
        {
            string imgUrl = "";
            if(this.ImgUrls == null || this.ImgUrls.Length == 0)
            {
                imgUrl = "http://www.intl-spectrum.com/articles/r75/ArticleDefault.jpg";
            }
            else
            {
                //assign to first img
                imgUrl = this.ImgUrls.Split(',')[0];
            }
            return imgUrl;
        }

    }
}
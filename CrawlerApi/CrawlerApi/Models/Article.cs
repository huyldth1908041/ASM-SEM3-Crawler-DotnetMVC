using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawlerApi.Models
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
        public ArticleStatus Status { get; set; }
        public enum ArticleStatus
        {
            PENDING = 0,
            DELETED = -1,
            ACTIVE = 1
        }
        //foreing key
        public int CategoryId { get; set; }
        //naviagtion property
        public virtual Category Category { get; set; }

        public virtual ICollection<UserRecentViewArticle> UserRecents { get; set; }

        public virtual ICollection<UserSavedArticle> UserSaveds { get; set; }
    }
}
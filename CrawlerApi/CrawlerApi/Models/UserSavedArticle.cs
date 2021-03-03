using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawlerApi.Models
{
    public class UserSavedArticle
    {
        public int Id { get; set; }
        public DateTime SavedAt { get; set; }
        public virtual AppUser User { get; set; }
        public virtual Article Article { get; set; }
    }
}
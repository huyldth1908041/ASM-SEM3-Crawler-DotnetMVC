using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawlerApi.Models
{
    public class AppUser: IdentityUser
    {
        public string Address { get; set; }
        public virtual ICollection<UserRecentViewArticle> RecentViewArticles { get; set; }
        public virtual ICollection<UserSavedArticle> SavedAritcles { get; set; }
    }
}
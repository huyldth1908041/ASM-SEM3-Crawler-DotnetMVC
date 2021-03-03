using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CrawlerApi.Models
{
    public class OwinApiContext : IdentityDbContext
    {
        public OwinApiContext() : base("OwinAuthDbContext")
        {

        }

        public static OwinApiContext Create()
        {
            return new OwinApiContext();
        }
        public DbSet<Article> Articles { get; set; }
        public DbSet<CrawlerConfig> CrawlerConfigs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }

    }
}
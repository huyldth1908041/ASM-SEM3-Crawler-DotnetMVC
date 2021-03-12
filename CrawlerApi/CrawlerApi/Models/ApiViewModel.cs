using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrawlerApi.Models
{
    public class UserInfoViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class CrawlerConfigViewModel 
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
    }

    public class ArticleViewModel
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
    }

    public class PreviewArticleViewModel
    {
        public string Title { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public string Description { get; set; }
    
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadNewsWebClient.API
{
    public class ApiEndPoint
    {
        public static string ApiDomain = "https://localhost:44394";
        public static string LoginPath = "/oauth/token";
        public static string GetUserInfoPath = "/api/Account/GetUserInfo";
        public static string RunUrlCrawlerPath = "/api/Bot/RunUrlCrawlerBot";
        public static string RunContentCrawlerPath = "/api/Bot/RunContentCrawlerBot";
        public static string GetListCrawlerConfigPath = "/api/CrawlerConfig/GetListConfig";
        public static string GetListPendingArticlePath = "/api/ArticleManager/GetListPendingArticle";
        public static string GetListAllArticlePath = "/api/ArticleManager/GetListAllArticle";
        public static string CreateConfigPath = "/api/ArticleManager/CreateConfig";
        public static string GetListCategoryPath = "/api/ArticleManager/GetListCategory";
        public static string UpdateAnArticlePath = "/api/ArticleManager/UpdateArticle";
        public static string CreateConfigPath = "/api/CrawlerConfig/CreateCrawlerConfig";
        public static string GetListCategoryPath = "/api/Category/GetListCategory";
    
        public static string GenerateGetArticleByIdUrl(int id)
        {
            var getArticleByIdPath = "/api/ArticleManager/GetArticleById/";
            return ApiDomain + getArticleByIdPath + id;
        }

       
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ReadNewsWebClient.API
{
    public class ApiEndPoint
    {
        public static string ApiDomain = "https://newsbeeapi.azurewebsites.net";
        public static string LoginPath = "/oauth/token";
        public static string GetUserInfoPath = "/api/Account/GetUserInfo";
        public static string RunUrlCrawlerPath = "/api/Bot/RunUrlCrawlerBot";
        public static string RunContentCrawlerPath = "/api/Bot/RunContentCrawlerBot";
        public static string GetListCrawlerConfigPath = "/api/CrawlerConfig/GetListConfig";
        public static string GetListPendingArticlePath = "/api/ArticleManager/GetListPendingArticle";
        public static string GetListAllArticlePath = "/api/ArticleManager/GetListAllArticle";
 

  
        public static string CreateConfigPath = "/api/CrawlerConfig/CreateCrawlerConfig";
        public static string GetListCategoryPath = "/api/Category/GetListCategory";
    
        public static string GenerateGetArticleByIdUrl(int? id)
        {
            if (id == null) id = 1;
            var getArticleByIdPath = "/api/ArticleManager/GetArticleById/";
            return ApiDomain + getArticleByIdPath + id;
        }

        public static string GenerateUpdateAricleUrl(int id)
        {
            var updatePath = "/api/ArticleManager/UpdateArticle/";
            return ApiDomain + updatePath + id;
        }


        public static string CreateArticlePath = "/api/ArticleManager/CreateArticle";




        public static string GenerateSearchByKeywordUrl(string keyword)
        {
            var searchPath = "/api/ArticleManager/SearchByKeyword?keyword=";
            var queryString = "";
            if(keyword != null)
            {
               queryString = HttpUtility.ParseQueryString(keyword, Encoding.UTF8).ToString();
            }
     
            return ApiDomain + searchPath + queryString;
        }
    }
}
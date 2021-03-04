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
    }
}
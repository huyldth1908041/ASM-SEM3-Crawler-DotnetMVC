using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UrlCrawlerBot.Helpers
{
    public class ValidateHelper
    {
        public static bool IsUrlValid(string url)
        {
            //url must start whith http:// or https://
            //following set of character a -> z A -> Z (non-case sensitive) or . / _ - whith length >= 5
            //url must end wihh . and a set of character a-z(case sensitive) wiht lengh 3 -> 5
            //ex: https://example.com is match
            //ex: /example not match
            //ex: http://example/path is not match
            //ex: http://example/path.html is math
            string regexPattern = @"[A-z0-9-_+\.\/]{5,}\.[a-z]{3,5}$";
            return Regex.IsMatch(url, regexPattern);
        }
    }
}

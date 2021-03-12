using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CrawlerApi.Helpers
{
    public class StringCompareHelper
    {
        public static bool CheckContainsIgnoreCase(string rootString, string searchString)
        {
            var culture = CultureInfo.InvariantCulture;
            if (culture.CompareInfo.IndexOf(rootString, searchString, CompareOptions.IgnoreCase) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
using ReadNewsWebClient.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReadNewsWebClient.Controllers
{
    [InitSessionFilter]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            return View();
        }
        [AdminFilter]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [AuthenticationFilter]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
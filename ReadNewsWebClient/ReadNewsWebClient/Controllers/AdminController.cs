using ReadNewsWebClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReadNewsWebClient.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateConfig()
        {
            return View();
        }

        public ActionResult ListResource()
        {

            var list = new List<CrawlerConfigsViewModel>()
            {
             new CrawlerConfigsViewModel(){
                 Path ="doi-sang",
                 Route="vnespress"
             }
            };
            return View(list);
        }

        public void  RunUrlCrawler()
        {

        }

        public void RunContentCrawler()
        {

        }

    }
}
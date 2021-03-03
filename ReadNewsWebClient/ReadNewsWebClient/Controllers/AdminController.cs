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

        public ActionResult ListCategory()
        {
            var list = new List<Category>()
            {
                new Category{

                    Name= "doi-song",
                    Id = 1
                }
            };
            return View(list);
        }

    }
}
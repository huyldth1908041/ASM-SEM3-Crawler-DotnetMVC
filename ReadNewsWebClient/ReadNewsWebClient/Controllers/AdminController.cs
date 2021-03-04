using Newtonsoft.Json;
using ReadNewsWebClient.API;
using ReadNewsWebClient.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
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
            var list = new List<CrawlerConfigsViewModel>();
            var getListResourceUrl = ApiEndPoint.ApiDomain + ApiEndPoint.GetListCrawlerConfigPath;
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {


                    HttpResponseMessage getListResult = httpClient.GetAsync(getListResourceUrl).Result;
                    if (!getListResult.IsSuccessStatusCode)
                    {

                        //request failed
                        TempData["GetListStatus"] = "Run url crawler Failed!";
                        return View(list);
                    }

                    var jsonResult = getListResult.Content.ReadAsStringAsync().Result;
                    var listConfigs = JsonConvert.DeserializeObject<List<CrawlerConfigsViewModel>>(jsonResult);
                    list = listConfigs;
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                TempData["GetListStatus"] = "Can not connect to API";

            }
            return View(list);
        }

        public ActionResult RunUrlCrawler()
        {
            var runUlrApiUrl = ApiEndPoint.ApiDomain + ApiEndPoint.RunUrlCrawlerPath;
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {


                    HttpResponseMessage runResult = httpClient.PostAsync(runUlrApiUrl, null).Result;
                    if (!runResult.IsSuccessStatusCode)
                    {

                        //request failed
                        TempData["RunCrawlerStatus"] = "Run url crawler Failed!";
                        return RedirectToAction("ListResource");
                    }
                    else
                    {
                        TempData["RunCrawlerStatus"] = "Run url crawler Sucess!";
                        return RedirectToAction("ListResource");
                    }
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                TempData["RunCrawlerStatus"] = "Can not connect to API";
                return RedirectToAction("ListResource");
            }
        }

        public ActionResult RunContentCrawler()
        {
            var runContentCrawlerApiUrl = ApiEndPoint.ApiDomain + ApiEndPoint.RunContentCrawlerPath;
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {


                    HttpResponseMessage runResult = httpClient.PostAsync(runContentCrawlerApiUrl, null).Result;
                    if (!runResult.IsSuccessStatusCode)
                    {

                        //request failed
                        TempData["RunCrawlerStatus"] = "Run content crawler Failed!";
                        return RedirectToAction("ListResource");
                    }
                    else
                    {
                        TempData["RunCrawlerStatus"] = "Run content crawler Sucess!";
                        return RedirectToAction("ListResource");
                    }
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                TempData["RunCrawlerStatus"] = "Can not connect to API";
                return RedirectToAction("ListResource");
            }

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

        public ActionResult ListNewArticle()
        {
            return View();
        }

    }
}
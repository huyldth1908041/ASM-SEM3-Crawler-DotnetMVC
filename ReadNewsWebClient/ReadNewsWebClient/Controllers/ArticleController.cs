using Newtonsoft.Json;
using ReadNewsWebClient.API;
using ReadNewsWebClient.Filters;
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
    [InitSessionFilter]
    public class ArticleController : Controller
    {

        
        // GET: Article
        public ActionResult Index()
        {
            var listAllArticle =  GetListArticle();
            var topFiveLatest = (from a in listAllArticle orderby a.CreatedAt select a).Take(5).ToList();
            var listCategory = GetCategory();
            var trendingArticle = (from a in listAllArticle orderby a.CreatedAt select a).Take(5).ToList();
            ArticleIndexViewModel model = new ArticleIndexViewModel()
            {
                AllArticles = listAllArticle,
                ListCategories = listCategory,
                TopFivesLatest = topFiveLatest,
                TrendingAricles = trendingArticle
            };
            //foreach(var item in model.AllArticles.ToList())
            //{
            //    if(item == null)
            //    {
            //        Debug.WriteLine("null");
            //    }
            //    else
            //    {
            //        Debug.WriteLine(item.Title);
            //    }
            //}
            return View(model);
        }

   


        public ActionResult Read(int id) 
        {
            //call api
            var url = ApiEndPoint.GenerateGetArticleByIdUrl(id);
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage runResult = httpClient.GetAsync(url).Result;
                    if (!runResult.IsSuccessStatusCode)
                    {
                        //request failed
                        TempData["AritcleDetailStatus"] = "Get article detais infor failed at index: " + id;
                        return RedirectToAction("ListPendingArticle");
                    }
                    else
                    {
                        //request success
                        var jsonString = runResult.Content.ReadAsStringAsync().Result;
                        var article = JsonConvert.DeserializeObject<Article>(jsonString);
                        ViewBag.listCategory = GetCategory();
                        ViewBag.listArticle = GetListArticle();
                        return View("Article", article);
                    }
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                TempData["AritcleDetailStatus"] = $"{err.Message} at index: {id}";
                return RedirectToAction("Index");
            }
            //var article = _articles[id];
            //ViewBag.listCategory = _categories;
            //ViewBag.listArticle = _articles;
            //return View("Article", article);
        }

        private  List<Article> GetListArticle()
        {
            var getListAllArticle = ApiEndPoint.ApiDomain + ApiEndPoint.GetListAllArticlePath;
            var listArticle = new List<Article>();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {


                    HttpResponseMessage runResult = httpClient.GetAsync(getListAllArticle).Result;
                    if (!runResult.IsSuccessStatusCode)
                    {
                        //request failed
                        Debug.WriteLine("Get list category failed");
                        
                    }
                    else
                    {
                        TempData["GetListPendingArticleStatus"] = "Get list pending article Sucess!";
                        var jsonString = runResult.Content.ReadAsStringAsync().Result;

                        var list = JsonConvert.DeserializeObject<List<Article>>(jsonString);
                        return list.ToList();

                        listArticle = JsonConvert.DeserializeObject<List<Article>>(jsonString);
                    

                    }
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                TempData["GetListPendingArticleStatus"] = "Can not connect to API";
              
            }
            return listArticle;
        }

        private List<Article> SaveArticles()
        {
            return null;
        }

        private List<Category> GetCategory()
        {
            var list = new List<Category>();
            var getListCategory = ApiEndPoint.ApiDomain + ApiEndPoint.GetListCategoryPath;
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {


                    HttpResponseMessage getListResult = httpClient.GetAsync(getListCategory).Result;
                    if (!getListResult.IsSuccessStatusCode)
                    {

                        //request failed
                        Debug.WriteLine("Null list cate");


                    }

                    var jsonResult = getListResult.Content.ReadAsStringAsync().Result;
                    var listCate = JsonConvert.DeserializeObject<List<Category>>(jsonResult);
                    list = listCate;

                }
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                Debug.WriteLine("Can not connect to API");

            }
            return list;

        }
    }
}
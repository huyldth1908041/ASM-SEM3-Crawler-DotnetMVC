using Newtonsoft.Json;
using PagedList;
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
        public ActionResult Index(int? page)
        {
            var listAllArticle =  GetListArticle();
            
            var topFiveLatest = (from a in listAllArticle orderby a.CreatedAt descending select a).Take(5).ToList();

            var listCategory = GetCategory();
            var trendingArticle = (from a in listAllArticle orderby a.CreatedAt descending select a).Take(5).ToList();
            //setting for paged list
            // 1. Tham số int? dùng để thể hiện null và kiểu int
            // page có thể có giá trị là null và kiểu int.

            // 2. Nếu page = null thì đặt lại là 1.
            if (page == null) page = 1;


            // 4. Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            int pageSize = 6;

            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);
            var pagedList = listAllArticle.ToPagedList<Article>(pageNumber, pageSize);
            ArticleIndexViewModel model = new ArticleIndexViewModel()
            {
                AllArticles = listAllArticle,
                ListCategories = listCategory,
                TopFivesLatest = topFiveLatest,
                TrendingAricles = trendingArticle,
                PagedListArticle = pagedList
            };
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

                        var jsonString = runResult.Content.ReadAsStringAsync().Result;

                        var list = JsonConvert.DeserializeObject<List<Article>>(jsonString);

                        List<Article> listArticleRaw = JsonConvert.DeserializeObject<List<Article>>(jsonString);
                        listArticle = listArticleRaw.Where(a => a.Status == 1).OrderByDescending(a => a.UpdatedAt).ToList();
                    }
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
         
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
        [HttpGet]
        public ActionResult Search(string keyword, int? page)
        {
            var apiEndPoint = ApiEndPoint.GenerateSearchByKeywordUrl(keyword);
            var listResult = new List<Article>();
            //setting for paged list
            // 1. Tham số int? dùng để thể hiện null và kiểu int
            // page có thể có giá trị là null và kiểu int.

            // 2. Nếu page = null thì đặt lại là 1.
            if (page == null) page = 1;


            // 4. Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            int pageSize = 6;

            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage getListResult = httpClient.GetAsync(apiEndPoint).Result;
                    if (!getListResult.IsSuccessStatusCode)
                    {

                        TempData["SearchStatus"] = "Not Found";
                        //request failed
                        Debug.WriteLine("No result found");


                    }

                    var jsonResult = getListResult.Content.ReadAsStringAsync().Result;
                    var listArticle = JsonConvert.DeserializeObject<List<Article>>(jsonResult);
                    listResult = listArticle;

                }
            }
            catch (Exception err)
            {
                TempData["SearchStatus"] = "Can not connect to API";
                Debug.WriteLine(err.Message);
                Debug.WriteLine("Can not connect to API");

            }
            ViewData["Keyword"] = keyword;
            IPagedList<Article> pagedList = null;
            if (listResult == null)
            {
                var empty = new List<Article>();
                pagedList =  empty.ToPagedList(pageNumber, pageSize);
            }
            pagedList = listResult.ToPagedList(pageNumber, pageSize);
            //get list category
            var listCategory = GetCategory();
            //get top three latest news
            var allArticle = GetListArticle();
            var topthree = (from a in allArticle orderby a.CreatedAt select a).Take(3).ToList();
            var viewModel = new SearchResultModel
            {
                ListArticle = pagedList,
                ListCategory = listCategory,
                TopThreeLatest = topthree
            };
            return View(viewModel);
           
        }
        public ActionResult Category(int? id, int? page)
        {
            //setting for paged list
            // 1. Tham số int? dùng để thể hiện null và kiểu int
            // page có thể có giá trị là null và kiểu int.

            // 2. Nếu page = null thì đặt lại là 1.
            if (page == null) page = 1;
            if (id == null) id = 1;

            // 4. Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            int pageSize = 6;

            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            var listAllRaw = GetListArticle();
            var listByCategory = listAllRaw.Where(a => a.CategoryId == id && a.Status == 1).OrderByDescending(a => a.CreatedAt).ToList();
            var pagedList = listByCategory.ToPagedList(pageNumber, pageSize);
            //get list category
            var listCategory = GetCategory();
            //get top three latest news
       
            var topthree = (from a in listAllRaw orderby a.CreatedAt select a).Take(3).ToList();
            var viewModel = new ListByCategoryModel
            {
                ListArticle = pagedList,
                ListCategory = listCategory,
                TopThreeLatest = topthree,
                CurrentCategory = listCategory.Find(x => x.Id == id)
            };
            return View("ListByCategory", viewModel);
        }
    }
}
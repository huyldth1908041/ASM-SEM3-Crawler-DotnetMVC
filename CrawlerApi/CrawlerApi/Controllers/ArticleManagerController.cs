using CrawlerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace CrawlerApi.Controllers
{
    public class ArticleManagerController : ApiController
    {

        private static OwinApiContext _db;
        public ArticleManagerController()
        {
            _db = new OwinApiContext();
        }
        // GET: ArticleManager
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetListPendingArticle()
        {
            List<Article> listPendingArticle = _db.Articles.Where(a => a.Status == 0).ToList();
          
            List<ArticleDataBindingModel> listArticleDataBindingModel = new List<ArticleDataBindingModel>();
            foreach (var item in listPendingArticle)
            {
                var bindingModel = new ArticleDataBindingModel()
                {
                    Id = item.Id,
                    CategoryId = item.CategoryId,
                    Content = item.Content,
                    CreatedAt = item.CreatedAt,
                    Description = item.Description,
                    ImgUrls = item.ImgUrls,
                    Link = item.Link,
                    Source = item.Source,
                    Status = (int)item.Status,
                    Title = item.Title,
                    UpdatedAt = item.UpdatedAt

                };
                listArticleDataBindingModel.Add(bindingModel);
            }

            return Json(listArticleDataBindingModel);
        
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult GetListAllArticle()
        {
            List<Article> listAllArticle = _db.Articles.ToList();
            List<ArticleDataBindingModel> listArticleDataBindingModel = new List<ArticleDataBindingModel>();
            foreach (var item in listAllArticle)
            {
                var bindingModel = new ArticleDataBindingModel()
                {
                    Id = item.Id,
                    CategoryId = item.CategoryId,
                    Content = item.Content,
                    CreatedAt = item.CreatedAt,
                    Description = item.Description,
                    ImgUrls = item.ImgUrls,
                    Link = item.Link,
                    Source = item.Source,
                    Status = (int)item.Status,
                    Title = item.Title,
                    UpdatedAt = item.UpdatedAt

                };
                listArticleDataBindingModel.Add(bindingModel);
            }

            return Json(listArticleDataBindingModel);
        }



    }
}
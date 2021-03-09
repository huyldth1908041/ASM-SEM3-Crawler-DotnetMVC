using CrawlerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;


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
        [HttpGet]
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

        [HttpGet]
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

        [HttpGet]
        public IHttpActionResult GetArticleById(int id)
        {
            var inDbArticle = _db.Articles.Find(id);
            if(inDbArticle == null)
            {
                return BadRequest();
            }
            var bindingArticle = new ArticleDataBindingModel()
            {
                CategoryId = inDbArticle.CategoryId,
                Content = inDbArticle.Content,
                CreatedAt = inDbArticle.CreatedAt,
                Description = inDbArticle.Description,
                Id = inDbArticle.Id,
                ImgUrls = inDbArticle.ImgUrls,
                Link = inDbArticle.Link,
                Source = inDbArticle.Source,
                Status = (int) inDbArticle.Status,
                Title = inDbArticle.Title,
                UpdatedAt = inDbArticle.UpdatedAt
            };
            return Json(bindingArticle);
        }

        

        public IHttpActionResult CreateConfig(CrawlerConfigDataBindingModel crawlerConfigDataBindingModel)
        {
            //to do check trung route+path
            //List<CrawlerConfig> existedCrawlerConfigs = _db.CrawlerConfigs.Where(c => c.Route == crawlerConfigDataBindingModel.Route).
            var newConfig = new CrawlerConfig() 
            {
                Route=crawlerConfigDataBindingModel.Route,
                CategoryId = crawlerConfigDataBindingModel.CategoryId,
                ContentSelector = crawlerConfigDataBindingModel.ContentSelector,
                DescriptionSelector = crawlerConfigDataBindingModel.DescriptionSelector,
                LinkSelector = crawlerConfigDataBindingModel.LinkSelector,
                RemovalSelector = crawlerConfigDataBindingModel.RemovalSelector,
                Path = crawlerConfigDataBindingModel.Path,
                TitleSelector =crawlerConfigDataBindingModel.TitleSelector
                    
            };
            _db.CrawlerConfigs.Add(newConfig);
            _db.SaveChanges();
            return Json(newConfig);
        }

        


    }
}
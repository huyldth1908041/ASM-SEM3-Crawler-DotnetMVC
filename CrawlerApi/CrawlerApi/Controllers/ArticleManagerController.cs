using CrawlerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Data.Entity;

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

            List<ArticleViewModel> listArticleDataBindingModel = new List<ArticleViewModel>();
            foreach (var item in listPendingArticle)
            {
                var bindingModel = new ArticleViewModel()
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
            List<ArticleViewModel> listArticleDataBindingModel = new List<ArticleViewModel>();
            foreach (var item in listAllArticle)
            {
                var bindingModel = new ArticleViewModel()
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
            if (inDbArticle == null)
            {
                return BadRequest();
            }
            var bindingArticle = new ArticleViewModel()
            {
                CategoryId = inDbArticle.CategoryId,
                Content = inDbArticle.Content,
                CreatedAt = inDbArticle.CreatedAt,
                Description = inDbArticle.Description,
                Id = inDbArticle.Id,
                ImgUrls = inDbArticle.ImgUrls,
                Link = inDbArticle.Link,
                Source = inDbArticle.Source,
                Status = (int)inDbArticle.Status,
                Title = inDbArticle.Title,
                UpdatedAt = inDbArticle.UpdatedAt
            };
            return Json(bindingArticle);
        }



       

        [HttpPut]
        public IHttpActionResult UpdateArticle(int id ,ArticleDataBindingModel articleDataBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var article = _db.Articles.Find(id);

            if (article == null)
            {
                return NotFound();
            }

            article.Title = articleDataBindingModel.Title;
            article.Description = articleDataBindingModel.Description;
            article.Content = articleDataBindingModel.Content;
            article.Source = articleDataBindingModel.Source;
            article.Link = articleDataBindingModel.Link;
            article.ImgUrls = articleDataBindingModel.ImgUrls;
            article.UpdatedAt = DateTime.Now;
            article.Status = (Article.ArticleStatus)articleDataBindingModel.Status;
            article.CategoryId = articleDataBindingModel.CategoryId;

            var recordsChanged = _db.SaveChanges();
       
            //convert to view model
            var viewModel = new ArticleViewModel()
            {
                Id = article.Id,
                CategoryId = article.CategoryId,
                Content = article.Content,
                CreatedAt = article.CreatedAt,
                Description = article.Description,
                ImgUrls = article.ImgUrls,
                Link = article.Link,
                Source = article.Source,
                Status = (int)article.Status,
                Title = article.Title,
                UpdatedAt = article.UpdatedAt
            };
            return Json(viewModel);

        }
        


    }

}
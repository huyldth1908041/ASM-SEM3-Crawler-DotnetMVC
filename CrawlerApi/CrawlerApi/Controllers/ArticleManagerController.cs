using CrawlerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Data.Entity;
using System.Diagnostics;
using System.Text;
using System.Globalization;

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
        public IHttpActionResult UpdateArticle(int id, ArticleDataBindingModel articleDataBindingModel)
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
            if (articleDataBindingModel.Title != null)
            {
                article.Title = articleDataBindingModel.Title;
            }
            if (articleDataBindingModel.Description != null)
            {
                article.Description = articleDataBindingModel.Description;
            }

            if (articleDataBindingModel.Content != null)
            {
                article.Content = articleDataBindingModel.Content;
            }

            if (articleDataBindingModel.Source != null)
            {
                article.Source = articleDataBindingModel.Source;
            }

            if (articleDataBindingModel.Link != null)
            {
                article.Link = articleDataBindingModel.Link;
            }
            if (articleDataBindingModel.ImgUrls != null)
            {
                article.ImgUrls = articleDataBindingModel.ImgUrls;
            }

            article.UpdatedAt = DateTime.Now;
            article.Status = (Article.ArticleStatus)articleDataBindingModel.Status;
            if (articleDataBindingModel.CategoryId != 0)
            {
                article.CategoryId = articleDataBindingModel.CategoryId;
            }

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
        [HttpGet]
        public IHttpActionResult SearchByKeyword(string keyword)
        {
            if (keyword == null)
            {
                return BadRequest();
            }
            //parse keyword
            var decodedKeyword = HttpUtility.UrlDecode(keyword, Encoding.UTF8);
            Debug.WriteLine(decodedKeyword);
            var listArticle = _db.Articles.ToList();
            var searchResult = listArticle.Where(a => CheckContainsIgnoreCase(a.Content, decodedKeyword) || CheckContainsIgnoreCase(a.Description, decodedKeyword) || CheckContainsIgnoreCase(a.Title, decodedKeyword)).ToList();
            if (searchResult == null || searchResult.Count() == 0)
            {
                return NotFound();
            }
            //convert to view model
            var listViewModel = new List<ArticleViewModel>();
            foreach (var item in searchResult)
            {
                var viewModel = new ArticleViewModel()
                {
                    CategoryId = item.CategoryId,
                    Content = item.Content,
                    CreatedAt = item.CreatedAt,
                    Description = item.Description,
                    Id = item.Id,
                    ImgUrls = item.ImgUrls,
                    Link = item.Link,
                    Source = item.Source,
                    Status = (int) item.Status,
                    Title = item.Title,
                    UpdatedAt = item.UpdatedAt
                };
                listViewModel.Add(viewModel);
            }
            return Json(listViewModel);

        }
        private static bool CheckContainsIgnoreCase(string rootString, string searchString)
        {
            var culture = CultureInfo.InvariantCulture;
            if(culture.CompareInfo.IndexOf(rootString, searchString, CompareOptions.IgnoreCase) >= 0)
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
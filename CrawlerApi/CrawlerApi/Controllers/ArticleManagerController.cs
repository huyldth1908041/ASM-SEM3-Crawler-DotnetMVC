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

        [System.Web.Http.HttpPut]
        public IHttpActionResult UpdateArticle(ArticleDataBindingModel articleDataBindingModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            var existingStudent = _db.Articles.Where(a => a.Id == articleDataBindingModel.Id)
                                                    .FirstOrDefault<Article>();

            if (existingStudent != null)
            {
                existingStudent.Id = articleDataBindingModel.Id;
                existingStudent.CategoryId = articleDataBindingModel.CategoryId;
                existingStudent.Content = articleDataBindingModel.Content;
                existingStudent.CreatedAt = articleDataBindingModel.CreatedAt;
                existingStudent.Description = articleDataBindingModel.Description;
                existingStudent.ImgUrls = articleDataBindingModel.ImgUrls;
                existingStudent.Link = articleDataBindingModel.Link;
                existingStudent.Source = articleDataBindingModel.Source;
                existingStudent.Status = (Article.ArticleStatus)articleDataBindingModel.Status;
                existingStudent.Title = articleDataBindingModel.Title;
                existingStudent.UpdatedAt = articleDataBindingModel.UpdatedAt;

                _db.SaveChanges();
            }
            else
            {
                return NotFound();
            }


            return Ok();
        }


        [System.Web.Http.HttpPut]
        public IHttpActionResult CreateArticle(ArticleDataBindingModel articleDataBindingModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            var existingStudent = _db.Articles.Where(a => a.Id == articleDataBindingModel.Id)
                                                    .FirstOrDefault<Article>();

            if (existingStudent == null)
            {
                existingStudent.Id = articleDataBindingModel.Id;
                existingStudent.CategoryId = articleDataBindingModel.CategoryId;
                existingStudent.Content = articleDataBindingModel.Content;
                existingStudent.CreatedAt = articleDataBindingModel.CreatedAt;
                existingStudent.Description = articleDataBindingModel.Description;
                existingStudent.ImgUrls = articleDataBindingModel.ImgUrls;
                existingStudent.Link = articleDataBindingModel.Link;
                existingStudent.Source = articleDataBindingModel.Source;
                existingStudent.Status = (Article.ArticleStatus)articleDataBindingModel.Status;
                existingStudent.Title = articleDataBindingModel.Title;
                existingStudent.UpdatedAt = articleDataBindingModel.UpdatedAt;

                _db.SaveChanges();
            }
            else
            {
                return NotFound();
            }


            return Ok();
        }

        //Create or update an category

        [System.Web.Http.HttpPost]
        public IHttpActionResult ManagerCategory(CategoryBindindModelToManager categoryBindindModelToManager)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            var existingCategory = _db.Categories.Where(a => a.Id == categoryBindindModelToManager.Id)
                                                    .FirstOrDefault<Category>();

            if(existingCategory != null)
            {
                existingCategory.Name = categoryBindindModelToManager.Name;
                _db.SaveChanges();
                return Ok();

            }
            else
            {
                _db.Categories.Add(new Category()
                {
                    Id = categoryBindindModelToManager.Id,
                    Name = categoryBindindModelToManager.Name
                });
                return Ok();

            }
        }

    }
}
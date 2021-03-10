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
            if (inDbArticle == null)
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
                Status = (int)inDbArticle.Status,
                Title = inDbArticle.Title,
                UpdatedAt = inDbArticle.UpdatedAt
            };
            return Json(bindingArticle);
        }


        [HttpGet]
        public IHttpActionResult GetListCategory()
        {
            List<Category> listCate = _db.Categories.ToList();
            List<CategoryBindindModel> listCategoryBindindModels = new List<CategoryBindindModel>();
            foreach (var item in listCate)
            {
                listCategoryBindindModels.Add(new CategoryBindindModel()
                {
                    Id = item.Id,
                    Name = item.Name
                });
            }
            return Json(listCategoryBindindModels);
        }

        [HttpPost]
        public IHttpActionResult CreateConfig(CrawlerConfigDataBindingModel crawlerConfigDataBindingModel)
        {
            //to do check trung route+path
            //List<CrawlerConfig> existedCrawlerConfigs = _db.CrawlerConfigs.Where(c => c.Route == crawlerConfigDataBindingModel.Route).
            var newConfig = new CrawlerConfig()
            {
                Route = crawlerConfigDataBindingModel.Route,
                CategoryId = crawlerConfigDataBindingModel.CategoryId,
                ContentSelector = crawlerConfigDataBindingModel.ContentSelector,
                DescriptionSelector = crawlerConfigDataBindingModel.DescriptionSelector,
                LinkSelector = crawlerConfigDataBindingModel.LinkSelector,
                RemovalSelector = crawlerConfigDataBindingModel.RemovalSelector,
                Path = crawlerConfigDataBindingModel.Path,
                TitleSelector = crawlerConfigDataBindingModel.TitleSelector

            };
            _db.CrawlerConfigs.Add(newConfig);
            _db.SaveChanges();
            return Json(newConfig);
        }
        [HttpPost]
        //Create or update an category
        public IHttpActionResult CreateCategory(CategoryBindindModel categoryBindindModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Not a valid model");

            }

            var existingCategory = _db.Categories.Where(a => a.Id == categoryBindindModel.Id)
                                                    .FirstOrDefault<Category>();


            if (existingCategory != null)
            {
                existingCategory.Name = categoryBindindModel.Name;
                _db.SaveChanges();
                return Json(categoryBindindModel);

            }
            else
            {
                _db.Categories.Add(new Category()
                {
                    Id = categoryBindindModel.Id,
                    Name = categoryBindindModel.Name
                });
                return Json(categoryBindindModel);


            }
        }

        [HttpPut]
        public IHttpActionResult UpdateArticle(int id ,ArticleDataBindingModel articleDataBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var article = _db.Articles.Find(articleDataBindingModel.Id);

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
            //_db.Entry(article).State = EntityState.Modified;
            _db.SaveChanges();
            return Json(article);




        }


    }

}
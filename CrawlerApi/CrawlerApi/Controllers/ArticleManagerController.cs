﻿using CrawlerApi.Models;
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
using CrawlerApi.Helpers;

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
            var searchResult = listArticle.Where(a => StringCompareHelper.CheckContainsIgnoreCase(a.Content, decodedKeyword) || StringCompareHelper.CheckContainsIgnoreCase(a.Description, decodedKeyword) || StringCompareHelper.CheckContainsIgnoreCase(a.Title, decodedKeyword)).ToList();
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

        [HttpPost]
        public IHttpActionResult CreateArticle(ArticleDataBindingModel articleDataBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existedArticle = _db.Articles.Where(a => a.Link.Equals(articleDataBindingModel.Link)).FirstOrDefault();

            if (existedArticle != null)
            {

                return Conflict();
            }

            var newArticle = new Article()
            {
                Title = articleDataBindingModel.Title,
                Description = articleDataBindingModel.Description,
                Content = articleDataBindingModel.Content,
                Source = articleDataBindingModel.Source,
                Link = articleDataBindingModel.Link,
                ImgUrls = articleDataBindingModel.ImgUrls,
                Status = (Article.ArticleStatus)articleDataBindingModel.Status,
                CategoryId = articleDataBindingModel.CategoryId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var article = _db.Articles.Add(newArticle);
            _db.SaveChanges();

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
            return Ok(viewModel);
        }

    }

}
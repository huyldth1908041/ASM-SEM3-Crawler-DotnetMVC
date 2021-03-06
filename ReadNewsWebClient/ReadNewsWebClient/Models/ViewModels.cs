﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReadNewsWebClient.Models
{

    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class UserInforViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string RollNumber { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }

    public class ArticleIndexViewModel
    {
        public List<Article> TopFivesLatest { get; set; }
        public List<Category> ListCategories { get; set; }
        public List<Article> AllArticles { get; set; }
        public PagedList.IPagedList<Article> PagedListArticle { get; set; }
        public List<Article> TrendingAricles { get; set; }

        public ArticleIndexViewModel()
        {
            TopFivesLatest = new List<Article>();
            ListCategories = new List<Category>();
            AllArticles = new List<Article>();
            TrendingAricles = new List<Article>();
        }

    }
    public class SearchResultModel
    {
        public PagedList.IPagedList<Article> ListArticle { get; set; }
        public List<Category> ListCategory { get; set; }
        public List<Article> TopThreeLatest { get; set; }

    }
    public class ListByCategoryModel
    {
        public PagedList.IPagedList<Article> ListArticle { get; set; }
        public List<Category> ListCategory { get; set; }
        public List<Article> TopThreeLatest { get; set; }
        public Category CurrentCategory { get; set; }
    }
    public class ReadArticleViewModel
    {
        public Article CurrentArticle { get; set; }
        public Category CurrentCategory { get; set; }
        public List<Article> TopFiveLatest { get; set; }
        public List<Article> RelatedArticles { get; set; }

    }

    public class RegisterViewModel
    {
        [DisplayName("Full Name")]
        [Required(ErrorMessage = "Please enter your name!")]
        public string FullName { get; set; }

        [DisplayName("Username")]
        [Required(ErrorMessage = "Please enter your username!")]
        public string Username { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "Please enter your email!")]
        [EmailAddress(ErrorMessage = "Invalid Email. Ex: your-email@example.com")]
        public string Email { get; set; }

        [DisplayName("Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a valid password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [Compare(otherProperty: "Password", ErrorMessage = "Password does not match!")]
        public string ConfirmPassword { get; set; }
    }
}
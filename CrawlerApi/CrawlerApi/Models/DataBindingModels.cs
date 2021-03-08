using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CrawlerApi.Models
{
    public class RegisterBindingModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }
    }

    public class UpdateUserInforBindingModel
    {
        [Required]
        [DisplayName("New Address")]
        public string Address { get; set; }
        [Phone]
        [Required]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }

    public class CreateRoleBingdingModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class AddUserToRoleBindingModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string RoleName { get; set; }
    }

    public class CrawlerConfigDataBindingModel
    {
        public int Id { get; set; }
        public string Route { get; set; }
        public string Path { get; set; }
        public string LinkSelector { get; set; }
        public string TitleSelector { get; set; }
        public string DescriptionSelector { get; set; }
        public string ContentSelector { get; set; }
        public string RemovalSelector { get; set; }

        public int CategoryId { get; set; }
        //navigation propery   
    }

    public class ArticleDataBindingModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Source { get; set; }
        public string Link { get; set; }
        public string ImgUrls { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Status { get; set; }
       
        //foreing key
        public int CategoryId { get; set; }
    }

    public class CategoryBindindModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
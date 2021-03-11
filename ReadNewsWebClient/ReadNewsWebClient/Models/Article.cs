using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReadNewsWebClient.Models
{
    public class Article
    {
        public int Id { get; set; }
        [Display(Name ="Tiêu đề")]
        public string Title { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [AllowHtml]
        [Display(Name = "Nội dung")]
        public string Content { get; set; }
        [Display(Name = "Nguồn")]
        public string Source { get; set; }
        [Display(Name = "Link gốc")]
        public string Link { get; set; }
        [Display(Name = "Link ảnh")]
        public string ImgUrls { get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime UpdatedAt { get; set; }
        [Display(Name = "Ngày sửa")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Trạng thái")]
        public int Status { get; set; } // 0 pending || 1 active || -1 deleted

        //foreing key
        public int CategoryId { get; set; }

        public string GetImageUrl()
        {
            string imgUrl = "";
            if(this.ImgUrls == null || this.ImgUrls.Length == 0)
            {
                imgUrl = "http://www.intl-spectrum.com/articles/r75/ArticleDefault.jpg";
            }
            else
            {
                //assign to first img
                imgUrl = this.ImgUrls.Split(',')[0];
            }
            return imgUrl;
        }

    }
}
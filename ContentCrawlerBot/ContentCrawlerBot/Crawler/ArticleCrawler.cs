using ContentCrawlerBot.Models;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ContentCrawlerBot.Crawler
{
    public class ArticleCrawler
    {
        public string Link { get; set; }
        public string TitleSelector { get; set; }
        public string ContentSelector { get; set; }
        public string DescriptionSelector { get; set; }
        public string RemovalSelector { get; set; }
        public int CategoryId { get; set; }
        public string Source { get; set; }

        private static HtmlWeb _htmlWeb = new HtmlWeb()
        {
            AutoDetectEncoding = false,
            OverrideEncoding = Encoding.UTF8,  //Set UTF8 để hiển thị tiếng Việt
            PreRequest = request =>
            {
                //support GZip
                // Make any changes to the request object that will be used.
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                return true;
            }
        };

        public Article CrawlArticle()
        {
            //load html
            var document = GetHtmlDocument();
            if(document == null)
            {
                return null;
            }
            //get title
            var title = GetTitle(document);
            var description = GetDescription(document);
            var content = GetContent(document);
            //get img urls
            var imgs = GetImageUrls(document);
            var article = new Article
            {
                Title = title,
                Content = content,
                Description = description,
                ImgUrls = imgs,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Link = this.Link,
                CategoryId = this.CategoryId,
                Status = 0,
                Source = this.Source
                
            };
            return article;
        }
        public ArticleCrawler(string link, string titleSelector, string contentSelector, string descriptionSelector, string removalSelector, int categoryId)
        {
            Link = link;
            TitleSelector = titleSelector;
            ContentSelector = contentSelector;
            DescriptionSelector = descriptionSelector;
            RemovalSelector = removalSelector;
            CategoryId = categoryId;
        }
        public ArticleCrawler()
        {

        }
        public HtmlDocument GetHtmlDocument()
        {
            Console.WriteLine("Getting HTML Document: " + Link);
            try
            {
                HtmlDocument document = _htmlWeb.Load(Link);

                return document; 
            }
            catch (Exception err)
            {

                Console.WriteLine(err + ": " + Link);
                return null;
            }

        }
        public string GetTitle(HtmlDocument document)
        {
            if (TitleSelector == null)
            {
                Console.WriteLine("Not found title selector: " + Link);
                return null;

            }
            var titleElement = document.DocumentNode.QuerySelector(TitleSelector);
            if (titleElement == null)
            {
                Console.WriteLine("Could not get title: " + Link);
                return null;
            }
            var title = titleElement.InnerText.Trim();
      
            return title;
        }

        public string GetContent(HtmlDocument document)
        {
            if (this.ContentSelector == null)
            {
                Console.WriteLine("Not found content selector: " + Link);
                return null;
            }


            if (RemovalSelector != null)
            {
                var removalElements = document.DocumentNode.QuerySelectorAll(RemovalSelector).ToList();
                foreach (var removeItem in removalElements)
                {
                    //remove from html document

                    removeItem.Remove();
                }
           
            }
            //get content
            var contentContainer = document.DocumentNode.QuerySelector(ContentSelector);

            if (contentContainer == null)
            {
                Console.WriteLine("Could not get content: " + Link);
                return null;
            }

            var listContentChildNode = contentContainer.ChildNodes.ToList();
            foreach (var node in listContentChildNode)
            {
                //skip blank text node
                if (node.Name.Equals("#text"))
                {
                    continue;
                }
                //scan for paragraph
                if (node.Name.Equals("p"))
                {
                    node.RemoveClass();
                    node.AddClass("article-paragraph");
                }
                //scan for img container node
                var imgElement = node.QuerySelector("img");
                if (imgElement != null)
                {
                    //get current img url and caption
                    var imgUrl =  GetImgUrl(imgElement);
                    var imgCaption = GetImgCaption(node);
                    //generate html code for img
                    GenerateHtmlForImg(imgUrl, imgCaption, node);
                }
            }


            return contentContainer.InnerHtml;
        }

        //get img urls
        public string GetImageUrls(HtmlDocument document)
        {
            //find img in content 
            var contentElement = document.DocumentNode.QuerySelector(ContentSelector);
            if(contentElement == null)
            {
                Console.WriteLine("Could not get images - Can not get content: " + Link);
                return null;
            }
            var imgEls = contentElement.QuerySelectorAll("img");
          
            if(imgEls == null || imgEls.Count() == 0)
            {
                Console.WriteLine("Article has no image: " + Link);
                return null;
            }
            var imgUrlStringBuilder = new StringBuilder();
            foreach (var imgEl in imgEls)
            {
                var url = GetImgUrl(imgEl);
                imgUrlStringBuilder.Append(url);
                imgUrlStringBuilder.Append(",");
            }
            //remove last coma
            if(imgUrlStringBuilder.Length > 0)
            {
                imgUrlStringBuilder.Length--;
            }
           
            return imgUrlStringBuilder.ToString();
        }
        //get article description
        public string GetDescription(HtmlDocument document)
        {
            if(DescriptionSelector == null)
            {
                Console.WriteLine("Could not found description selector: " + Link);
                return null;
            }
            var descriptionEl = document.DocumentNode.QuerySelector(DescriptionSelector);
            if(descriptionEl == null)
            {
                Console.WriteLine("Get description failed: " + Link);
                return null;
            }
            var description = descriptionEl.InnerText.Trim();
         
            return description;
        }

        //find img url in an img node
        private string GetImgUrl(HtmlNode imgNode)
        {
            string imgUrl = "";
            imgUrl = imgNode.Attributes["src"].Value;
            if (imgUrl.Contains("base64"))
            {
                imgUrl = imgNode.Attributes["data-src"].Value;

            }
            return imgUrl;
        }
        //generate html tag for hold img and caption and custom class name
        private void GenerateHtmlForImg(string url, string caption, HtmlNode originNode)
        {
            //generate html
            originNode.Name = "figure";
            if(originNode.Attributes != null)
            {
                originNode.Attributes.RemoveAll();
            }
            originNode.RemoveClass();
            originNode.AddClass("img-container");
            originNode.InnerHtml = $"<img src='{url}'  class = 'article-img'>" +
                $"<figcaption><p class='img-caption'>{caption}</figcaption >";

        }
        //get caption in an img container node
        private string GetImgCaption(HtmlNode imgContainerNode)
        {
            //get current img caption
            var imgCaptionElement = imgContainerNode.QuerySelector("p");
            string imgCaption = "";

            if (imgCaptionElement != null)
            {
                imgCaption = imgCaptionElement.InnerText.Trim();

            }
            return imgCaption;
        }
    }
}

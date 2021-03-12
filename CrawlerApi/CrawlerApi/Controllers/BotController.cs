using Azure.Storage.Queues;
using CrawlerApi.Helpers;
using CrawlerApi.Models;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace CrawlerApi.Controllers
{
    public class BotController : ApiController
    {
        private static HtmlWeb _htmlWeb;
        private static OwinApiContext _db;
        public BotController()
        {
            _htmlWeb = new HtmlWeb()
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
            _db = new OwinApiContext();
        }
        [HttpPost]
        public IHttpActionResult RunUrlCrawlerBot()
        {
            var queueName = "runurlqueue";
            try
            {
                // Get the connection string from app settings
                string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

                // Instantiate a QueueClient which will be used to create and manipulate the queue
                QueueClient queueClient = new QueueClient(connectionString, queueName);

                // Create the queue
                queueClient.CreateIfNotExists();

                if (queueClient.Exists())
                {
                    Console.WriteLine($"Queue created: '{queueClient.Name}'");
                    var message = "Run request: " + DateTime.Now.ToLongTimeString();
                    queueClient.SendMessage(message);
                    Debug.WriteLine("Sent: " + message);
                    return Ok();
                }
                else
                {
                    Debug.WriteLine($"Make sure the Azurite storage emulator running and try again.");
                    return BadRequest();

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}\n\n");
                Debug.WriteLine($"Make sure the Azurite storage emulator running and try again.");
                return BadRequest();

            }

        }


        [HttpPost]
        public IHttpActionResult RunContentCrawlerBot()
        {
            var queueName = "runcontentqueue";
            try
            {
                // Get the connection string from app settings
                string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

                // Instantiate a QueueClient which will be used to create and manipulate the queue
                QueueClient queueClient = new QueueClient(connectionString, queueName);

                // Create the queue
                queueClient.CreateIfNotExists();

                if (queueClient.Exists())
                {
                    Console.WriteLine($"Queue created: '{queueClient.Name}'");
                    var message = "Run request: " + DateTime.Now.ToLongTimeString();
                    queueClient.SendMessage(message);
                    Debug.WriteLine("Sent: " + message);
                    return Ok();
                }
                else
                {
                    Debug.WriteLine($"Make sure the Azurite storage emulator running and try again.");
                    return BadRequest();

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}\n\n");
                Debug.WriteLine($"Make sure the Azurite storage emulator running and try again.");

                return BadRequest();

            }

        }
        public IHttpActionResult PreviewUrlCrawler(PreviewUrlCrawlerBindingModel model)
        {
            Debug.WriteLine("Hello");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var setLink = GetArticleUrls(model);
            return Json(setLink);
        }

        public IHttpActionResult PreviewArticle(PreviewArticleBindingModel model)
        {
            Debug.WriteLine("Hello preview article");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            HtmlDocument document = null;

            var url = model.Link;
            try
            {
                document = _htmlWeb.Load(url);
            }
            catch (Exception err)
            {
                Console.WriteLine("LOAD HTML DOC FAILED: " + err.Message);
                return Content(HttpStatusCode.InternalServerError, "can not get html document");

            }
            //get title
            var title = GetTitle(document, model);
            var description = GetDescription(document, model);
            var content = GetContent(document, model);
            var viewModel = new PreviewArticleViewModel()
            {
                Content = content,
                Description = description,
                Title = title
            };
            return Json(viewModel);
        }

        private HashSet<string> GetArticleUrls(PreviewUrlCrawlerBindingModel config)
        {
            //sử dụng set để k bị duplicate
            HashSet<string> setLink = new HashSet<string>();
            //Load trang web, nạp html vào document

            HtmlDocument document = null;

            var url = config.Route + config.Path;
            try
            {
                document = _htmlWeb.Load(url);
            }
            catch (Exception err)
            {
                Console.WriteLine("LOAD HTML DOC FAILED: " + err.Message);
                return setLink;

            }
            //lấy ra toàn bộ thẻ a
            var aItems = document.DocumentNode.QuerySelectorAll(config.LinkSelector).ToList();
            var ExistedLinks = from article in _db.Articles select article.Link;
            //lấy aritcle url và lưu vào set
            foreach (var item in aItems)
            {
                var hrefValue = item.Attributes["href"].Value;
                var link = "";
                //validate url
                if (!link.Contains(config.Route))
                {
                    link = config.Route + hrefValue;
                }
                else
                {
                    link = hrefValue;
                }
                if (!ValidateHelper.IsUrlValid(link))
                {
                    Console.WriteLine($"[Not valid]: {link}");
                    continue;
                }
                //check link duplicate trong db
                if (ExistedLinks.Contains(link))
                {
                    Console.WriteLine($"[Existed]: {link}");
                    continue;
                }
                Console.WriteLine($"[Get success]: {link}");
                setLink.Add(link);
            }

            return setLink;

        }
        //for content crawler
        private string GetTitle(HtmlDocument document, PreviewArticleBindingModel config)
        {
            if (config.TitleSelector == null)
            {
     
                return null;

            }
            var titleElement = document.DocumentNode.QuerySelector(config.TitleSelector);
            if (titleElement == null)
            {
           
                return null;
            }
            var title = titleElement.InnerText.Trim();

            return title;
        }

        private string GetContent(HtmlDocument document, PreviewArticleBindingModel config)
        {
            if (config.ContentSelector == null)
            {
         
                return null;
            }


            if (config.RemovalSelector != null && config.RemovalSelector.Length != 0)
            {
                var removalElements = document.DocumentNode.QuerySelectorAll(config.RemovalSelector).ToList();
                foreach (var removeItem in removalElements)
                {
                    //remove from html document

                    removeItem.Remove();
                }

            }
            //get content
            var contentContainer = document.DocumentNode.QuerySelector(config.ContentSelector);

            if (contentContainer == null)
            {
        
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
                    var imgUrl = GetImgUrl(imgElement);
                    var imgCaption = GetImgCaption(node);
                    //generate html code for img
                    GenerateHtmlForImg(imgUrl, imgCaption, node);
                }
            }


            return contentContainer.InnerHtml;
        }

        //get img urls
        private string GetImageUrls(HtmlDocument document, PreviewArticleBindingModel config)
        {
            //find img in content 
            var contentElement = document.DocumentNode.QuerySelector(config.ContentSelector);
            if (contentElement == null)
            {
     
                return null;
            }
            var imgEls = contentElement.QuerySelectorAll("img");

            if (imgEls == null || imgEls.Count() == 0)
            {

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
            if (imgUrlStringBuilder.Length > 0)
            {
                imgUrlStringBuilder.Length--;
            }

            return imgUrlStringBuilder.ToString();
        }
        //get article description
        private string GetDescription(HtmlDocument document, PreviewArticleBindingModel config)
        {
            if (config.DescriptionSelector == null)
            {
  
                return null;
            }
            var descriptionEl = document.DocumentNode.QuerySelector(config.DescriptionSelector);
            if (descriptionEl == null)
            {
         
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
            if (originNode.Attributes != null)
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

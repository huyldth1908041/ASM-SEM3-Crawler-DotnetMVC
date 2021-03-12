using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlCrawlerBot.Helpers;
using UrlCrawlerBot.Models;

namespace UrlCrawlerBot.Crawler
{
    class UrlCrawler
    {
        private static UrlCrawlerBotDbContext _db;
        private static ConnectionFactory _factory;
        private static HtmlWeb _htmlWeb;
        private static IConnection _connection;
        private static int _pushCount;
        public UrlCrawler()
        {
            _db = new UrlCrawlerBotDbContext();
            _factory = new ConnectionFactory() { HostName = "localhost" };
            _htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8  //Set UTF8 để hiển thị tiếng Việt
            };
            _pushCount = 0;
        }

        public void Run()
        {
            var listConfig = _db.CrawlerConfigs.ToList();
           
            foreach (var config in listConfig)
            {
         
                var listUrl = GetArticleUrls(config);

                foreach (var item in listUrl)
                {
                    var packed = PackConfigToJson(config, item);
                    PushPackConfigToQueue(packed);
                }
            }
            if (_connection != null)
            {
                //close if existe
                Console.WriteLine("[Closed connection]");
                _connection.Close();
            }
            Console.WriteLine($"Sent succces {_pushCount} package(s) to queue");
        }

        private HashSet<string> GetArticleUrls(CrawlerConfig config)
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
        private string PackConfigToJson(CrawlerConfig config, string url)
        {
            var packed = new
            {
                Link = url,
                ConfigId = config.Id
            };
            var jsonPacked = JsonConvert.SerializeObject(packed);
            Console.WriteLine("[Packed]: " + url);
            return jsonPacked;
        }

        private void PushPackConfigToQueue(string pack)
        {
            var cnn = GetConnection();
            if (cnn == null)
            {
                Console.WriteLine("[Sent failed]: " + pack);
                return;
            }
            var channel = cnn.CreateModel();

            channel.QueueDeclare(queue: "amqHelloRabbitMq",
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);


            var body = Encoding.UTF8.GetBytes(pack);

            channel.BasicPublish(exchange: "",
                                 routingKey: "amqHelloRabbitMq",
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine("[Sent]: " + pack);
            channel.Close();
            _pushCount++;
        }
        //helper
        private IConnection GetConnection()
        {
            if (_connection != null)
            {
                return _connection;
            }
            try
            {
                Console.WriteLine("[Create new connecion success]");
                _connection = _factory.CreateConnection();
            }
            catch (Exception err)
            {
                Console.WriteLine("[Get connection to queue failed]: " + err.Message);
                _connection = null;
                return null;
            }
            return _connection;
        }
    }
}

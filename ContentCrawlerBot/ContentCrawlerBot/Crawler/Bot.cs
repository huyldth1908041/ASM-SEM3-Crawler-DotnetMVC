using ContentCrawlerBot.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentCrawlerBot.Crawler
{
    public class Bot
    {
        private static ContentCrawlerDbContext _db;

      
        private static ArticleCrawler _articleCrawler;
        private static int _currentConfigId;
        public Bot()
        {
            _db = new ContentCrawlerDbContext();
         
            _articleCrawler = null;
            _currentConfigId = 0;
        }
        public void Run()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "amqHelloRabbitMq",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var jsonData = Encoding.UTF8.GetString(body);
                    var crawlerInfo = GetCrawlerInfor(jsonData);

                    var messageConfigId = crawlerInfo.ConfigId;
               
                    if (_currentConfigId == 0)
                    {
                        Console.WriteLine("[Init cache]");
                        //init crawler
                        _articleCrawler = CreateArticleCrawler(crawlerInfo);
                        //init current config id
                        _currentConfigId = messageConfigId;
                    }
                    else if (messageConfigId != _currentConfigId)
                    {
                        Console.WriteLine($"[Update cache]");
                        //update crawler
                        _articleCrawler = CreateArticleCrawler(crawlerInfo);
                        //update config id
                        _currentConfigId = messageConfigId;
                    }
                    //craw article
                    //update curent crawler link
                    _articleCrawler.Link = crawlerInfo.Link;
                    //crawl article
                    var article = _articleCrawler.CrawlArticle();
                    if(article == null)
                    {
                        Console.WriteLine("[Crawled Failed] " + crawlerInfo.Link);
                        return;
                    }
                    //save to db
                    _db.Articles.Add(article);
                    _db.SaveChanges();
                    Console.WriteLine("[Saved] " + article.Title);
                };
                channel.BasicConsume(queue: "amqHelloRabbitMq",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }

        }

        private ArticleCrawler CreateArticleCrawler(CrawlerInfor crawlerInfo)
        {
            Console.WriteLine("[Getting Crawler Configs in database]");
            var inDbConfigs = _db.CrawlerConfigs.Find(crawlerInfo.ConfigId);
            Console.WriteLine("[Creating new ArticleCrawler Instance]");
            var crawler = new ArticleCrawler()
            {
                CategoryId = inDbConfigs.CategoryId,
                ContentSelector = inDbConfigs.ContentSelector,
                DescriptionSelector = inDbConfigs.DescriptionSelector,
          
                RemovalSelector = inDbConfigs.RemovalSelector,
                Source = inDbConfigs.Route,
                TitleSelector = inDbConfigs.TitleSelector
            };
            return crawler;
        }

        private static CrawlerInfor GetCrawlerInfor(string jsonData)
        {
            return JsonConvert.DeserializeObject<CrawlerInfor>(jsonData);
        }


    }
}

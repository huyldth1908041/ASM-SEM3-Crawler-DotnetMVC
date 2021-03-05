using Azure.Storage.Queues;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CrawlerApi.Controllers
{
    public class BotController : ApiController
    {
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
    }
}

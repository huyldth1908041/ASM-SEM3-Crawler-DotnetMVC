using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UrlCrawlerBot.Crawler;
using UrlCrawlerBot.Helpers;
using UrlCrawlerBot.Models;

namespace UrlCrawlerBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            UrlCrawler bot = new UrlCrawler();
            //bot.Run();

            var queueName = "runurlqueue";
            // Get the connection string from app settings
            string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

            // Instantiate a QueueClient which will be used to manipulate the queue
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            //queue polling
            //if have messgae in queue then -> process messgae
            //if not -> waiting for specific time then stop 
            var currentInterval = 0;
            var maxInterval = 15;

            if (queueClient.Exists())
            {
                while (true)
                {
                    try
                    {
                        // Get the next message
                        QueueMessage[] retrievedMessage = queueClient.ReceiveMessages(1, TimeSpan.FromSeconds(120));
                        //get messge text
                        var message = retrievedMessage[0].MessageText;
                        // Process the message in less than 120 seconds
                        Console.WriteLine(message);
                        //run bot when received message
                        bot.Run();
                        //reset interval
                        currentInterval = 0;
                        Console.WriteLine(currentInterval);
                        // Delete the message
                        queueClient.DeleteMessage(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine(err.Message);
                        if (currentInterval < maxInterval)
                        {
                            currentInterval++;
                        }
                        else
                        {
                            break;
                        }
                        Console.WriteLine("waiting for " + currentInterval + "s");
                        Thread.Sleep(TimeSpan.FromSeconds(currentInterval));
                    }
                }
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }
    }
}

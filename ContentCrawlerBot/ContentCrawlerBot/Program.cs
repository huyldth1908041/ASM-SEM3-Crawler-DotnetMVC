﻿using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using ContentCrawlerBot.Crawler;
using ContentCrawlerBot.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContentCrawlerBot
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Bot bot = new Bot();
            var queueName = "runcontentqueue";
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
                        QueueMessage[] retrievedMessage = queueClient.ReceiveMessages();
                        //get messge text
                        var message = retrievedMessage[0].MessageText;
                        // Process (i.e. print) the message in less than 30 seconds
                        Console.WriteLine(message);
                        //run bot when received message
                        bot.Run();
                        //reset interval
                        currentInterval = 0;
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
            Console.ReadLine();
        }
    }
}

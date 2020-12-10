using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace SampleAzureServiceBus.Receiver
{
    public class ServiceBusMessageHandler
    {
        #region  properties
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
        #endregion

        #region constructors
        public ServiceBusMessageHandler() {}
        public ServiceBusMessageHandler(string connectionString, string queueName)
        {
            ConnectionString = connectionString;
            QueueName = queueName;
        }
        #endregion

        #region public methods
        public async Task ReceiveMessagesAsync()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                Console.WriteLine("ConnectionString is not set.");
                return;
            }
            if (string.IsNullOrEmpty(QueueName))
            {
                Console.WriteLine("QueueName is not set.");
                return;
            }

            await using (ServiceBusClient client = new ServiceBusClient(ConnectionString))
            {
                // create a processor that we can use to process the messages
                ServiceBusProcessor processor = client.CreateProcessor(QueueName, new ServiceBusProcessorOptions());

                // add handler to process messages
                processor.ProcessMessageAsync += MessageHandler;

                // add handler to process any errors
                processor.ProcessErrorAsync += ErrorHandler;

                // start processing 
                await processor.StartProcessingAsync();

                Console.WriteLine("Wait for a minute and then press any key to end the processing");
                Console.ReadKey();

                // stop processing 
                Console.WriteLine("\nStopping the receiver...");
                await processor.StopProcessingAsync();
                Console.WriteLine("Stopped receiving messages");
            }
        }
        #endregion

        #region private methods
        // handle received messages
        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Console.WriteLine($"Received: {body}");

            // complete the message. messages is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);
        }

        // handle any errors when receiving messages
        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
        #endregion
    }
}

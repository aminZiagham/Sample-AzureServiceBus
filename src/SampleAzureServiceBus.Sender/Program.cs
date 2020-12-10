using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace SampleAzureServiceBus.Sender
{
    class Program
    {
        static string connectionString = "Endpoint=sb://testmessagingbus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=tR4Dw+M/2IeRHzq2bL4vUNzcIduivG5keiF5vilrh/c=";  
        static string queueName = "eventbus";  

        static async Task Main(string[] args)
        {
            // send a message to the queue
            await SendMessageAsync();
        }

        static async Task SendMessageAsync()
        {
            // create a Service Bus client 
            await using (ServiceBusClient client = new ServiceBusClient(connectionString))
            {
                // create a sender for the queue 
                ServiceBusSender sender = client.CreateSender(queueName);

                // create a message that we can send
                ServiceBusMessage message = new ServiceBusMessage("Hello world!");

                // send the message
                await sender.SendMessageAsync(message);
                Console.WriteLine($"Sent a single message to the queue: {queueName}");
            }
        }
    }
}

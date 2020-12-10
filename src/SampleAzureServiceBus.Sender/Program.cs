using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SampleAzureServiceBus.Sender
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            
            // Or this approach to get your configuration
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            
            // Then get your values using this approach
            var appConfig = builder.GetSection("serviceBusOptions").Get<ServiceBusOptions>();;
            
            ServiceBusMessageHandler messageHandler = new ServiceBusMessageHandler(appConfig.ConnectionString,appConfig.QueueName);
            
            // send a message to the queue
            await messageHandler.SendMessageAsync("Test Message");

            await messageHandler.SendMessageBatchAsync();
        }
    }
}

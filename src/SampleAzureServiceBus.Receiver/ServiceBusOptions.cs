using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace SampleAzureServiceBus.Receiver
{
    public class ServiceBusOptions
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
    }
}

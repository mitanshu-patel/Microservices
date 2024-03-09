using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UserService.Services
{
    public class MessageDeliveryService : IMessageDeliveryService
    {
        public async Task PublishMessageAsync<T>(T requestBody, string topicName)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("local.settings.json")
              .Build();

            var options = new ServiceBusClientOptions {
                TransportType = ServiceBusTransportType.AmqpWebSockets,
                WebProxy = new WebProxy(Environment.GetEnvironmentVariable("WebProxy"), true)
             };

            var settingsSection = configuration.GetConnectionString("ServiceBusConnectionString");
            var client = new ServiceBusClient(settingsSection, options);
            var sender = client.CreateSender(topicName);
            var customBody = new
            {
                FullName = requestBody.GetType().FullName,
                Data = requestBody,
            };
            var body = JsonSerializer.Serialize(customBody);
            var message = new ServiceBusMessage(body);
            await sender.SendMessageAsync(message);
        }
    }
}

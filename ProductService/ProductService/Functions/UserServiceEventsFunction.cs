using System;
using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using ProductService.Common;
using UserService.Messages;

namespace ProductService.Functions
{
    public class UserServiceEventsFunction
    {
        [FunctionName("UserServiceEventsFunction")]
        public void Run([ServiceBusTrigger("userserviceevents", "userserviceproduct", Connection = "ServiceBusConnectionString")]string mySbMsg, ILogger log)
        {
            var response = JsonSerializer.Deserialize<MessageResult>(mySbMsg);
            if(response.FullName == typeof(UserDetailUpdated).FullName)
            {
                log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            }
        }
    }
}

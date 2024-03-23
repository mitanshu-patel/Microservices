using System;
using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using ProductService.Common;
using ProductService.Domain.Messaging.UserTokenEvent;
using ProductService.Services;
using UserService.Messages;

namespace ProductService.Functions
{
    public class UserServiceEventsFunction
    {
        private readonly ILogger<UserServiceEventsFunction> _logger;

        private readonly IMediator mediator;

        public UserServiceEventsFunction(IMediator mediator, ILogger<UserServiceEventsFunction> _logger)
        {
            this.mediator = mediator;
            this._logger = _logger;
        }

        [FunctionName("UserServiceEventsFunction")]
        public void Run([ServiceBusTrigger("userserviceevents", "userserviceproduct", Connection = "ServiceBusConnectionString")]string mySbMsg)
        {
            var response = JsonSerializer.Deserialize<MessageResult>(mySbMsg);
            if(response.FullName == typeof(UserTokenUpdated).FullName)
            {
                var request = JsonSerializer.Deserialize<UserTokenUpdated>(response.Data.ToString());
                this.mediator.SendAsync<UserTokenEventCommand, CustomResponse<UserTokenEventResult>>(new UserTokenEventCommand(request));
                this._logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            }
        }
    }
}

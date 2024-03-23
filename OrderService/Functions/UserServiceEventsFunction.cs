using System;
using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using OrderService.Common;
using OrderService.Domain.Messaging.UserDetailUpdated;
using OrderService.Domain.Messaging.UserTokenEvent;
using OrderService.Services;
using UserService.Messages;

namespace OrderService.Functions
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
        public void Run([ServiceBusTrigger("userserviceevents", "userserviceorder", Connection = "ServiceBusConnectionString")]string mySbMsg)
        {
            var response = JsonSerializer.Deserialize<MessageResult>(mySbMsg);
            if(response.FullName == typeof(UserTokenUpdated).FullName)
            {
                var request = JsonSerializer.Deserialize<UserTokenUpdated>(response.Data.ToString());
                this.mediator.SendAsync<UserTokenEventCommand, CustomResponse<UserTokenEventResult>>(new UserTokenEventCommand(request));
                this._logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            }

            if (response.FullName == typeof(UserDetailUpdated).FullName)
            {
                var request = JsonSerializer.Deserialize<UserDetailUpdated>(response.Data.ToString());
                this.mediator.SendAsync<UserDetailUpdatedCommand, CustomResponse<UserDetailUpdatedResult>>(new UserDetailUpdatedCommand(request));
                this._logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            }
        }
    }
}

using System;
using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using OrderService.Common;
using OrderService.Domain.Messaging.ProductDetailUpdated;
using OrderService.Domain.Messaging.UserTokenEvent;
using OrderService.Services;
using ProductService.Messages;

namespace OrderService.Functions
{
    public class ProductServiceEventsFunction
    {
        private readonly ILogger<ProductServiceEventsFunction> _logger;

        private readonly IMediator mediator;

        public ProductServiceEventsFunction(IMediator mediator, ILogger<ProductServiceEventsFunction> _logger)
        {
            this.mediator = mediator;
            this._logger = _logger;
        }

        [FunctionName("ProductServiceEventsFunction")]
        public void Run([ServiceBusTrigger("productserviceevents", "productserviceorder", Connection = "ServiceBusConnectionString")]string mySbMsg)
        {
            var response = JsonSerializer.Deserialize<MessageResult>(mySbMsg);
            if(response.FullName == typeof(ProductDetailUpdated).FullName)
            {
                var request = JsonSerializer.Deserialize<ProductDetailUpdated>(response.Data.ToString());
                this.mediator.SendAsync<ProductDetailUpdatedCommand, CustomResponse<ProductDetailUpdatedResult>>(new ProductDetailUpdatedCommand(request));
                this._logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            }
        }
    }
}

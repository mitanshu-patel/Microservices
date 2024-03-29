using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using OrderService.Common;
using OrderService.Domain.Orders.Add;
using OrderService.Domain.Orders.Get;
using OrderService.Domain.Orders.GetUserOrders;
using OrderService.Services;

namespace OrderService.Functions
{
    public class OrdersFunction
    {
        private readonly ILogger<OrdersFunction> _logger;

        private const string OpenApiTag = "Orders";

        private readonly IAuthenticationService authenticationService;
        private readonly IMediator mediator;

        public OrdersFunction(ILogger<OrdersFunction> log, IAuthenticationService authenticationService, IMediator mediator)
        {
            _logger = log;
            this.authenticationService = authenticationService;
            this.mediator = mediator;
        }

        [FunctionName("AddOrder")]
        [OpenApiOperation(operationId: "AddOrder", tags: OpenApiTag)]
        [OpenApiSecurity("BearerAuth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "Authorization")]
        [OpenApiRequestBody("application/json", typeof(AddOrderCommand))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(AddOrderResult), Description = "Adds new order")]
        public async Task<IActionResult> AddOrder(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/orders")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request for Add Order");
            var authResult = this.authenticationService.ValidateToken(req);
            if (!authResult.IsValid)
            {
                _logger.LogInformation("User unauthorized");
                return new UnauthorizedResult(); // No authentication info.
            }
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<AddOrderCommand>(requestBody);
            var result = await mediator.SendAsync<AddOrderCommand, CustomResponse<AddOrderResult>>(data);

            return result.GetResponse();
        }

        [FunctionName("GetOrder")]
        [OpenApiOperation(operationId: "GetOrder", tags: OpenApiTag)]
        [OpenApiSecurity("BearerAuth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "Authorization")]
        [OpenApiParameter("orderId", Type = typeof(int), Visibility = OpenApiVisibilityType.Important, Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(GetOrderDetailsResult), Description = "Gets existing order details")]
        public async Task<IActionResult> GetOrder(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/orders/{orderId:int}")] HttpRequest req, int orderId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request for Get Order");
            var authResult = this.authenticationService.ValidateToken(req);
            if (!authResult.IsValid)
            {
                _logger.LogInformation("User unauthorized");
                return new UnauthorizedResult(); // No authentication info.
            }
            var result = await mediator.SendAsync<GetOrderDetailsQuery, CustomResponse<GetOrderDetailsResult>>(new GetOrderDetailsQuery(orderId));

            return result.GetResponse();
        }

        [FunctionName("GetUserOrders")]
        [OpenApiOperation(operationId: "GetUserOrders", tags: OpenApiTag)]
        [OpenApiSecurity("BearerAuth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "Authorization")]
        [OpenApiParameter("userId", Type = typeof(int), Visibility = OpenApiVisibilityType.Important, Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(GetUserOrdersResult), Description = "Gets list of orders for given userid.")]
        public async Task<IActionResult> GetUserOrders(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/users/{userId:int}/orders")] HttpRequest req, int userId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request for Get User Orders");
            var authResult = this.authenticationService.ValidateToken(req);
            if (!authResult.IsValid)
            {
                _logger.LogInformation("User unauthorized");
                return new UnauthorizedResult(); // No authentication info.
            }
            var result = await mediator.SendAsync<GetUserOrdersQuery, CustomResponse<GetUserOrdersResult>>(new GetUserOrdersQuery(userId));

            return result.GetResponse();
        }
    }
}


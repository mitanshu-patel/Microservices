using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductService.Services;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using ProductService.Common;
using System.Net;
using ProductService.Domain.Products.AddProduct;
using ProductService.Domain.Products.GetProduct;
using ProductService.Domain.Products.GetProducts;
using ProductService.Domain.Products.Common.DTOs;
using ProductService.Domain.Products.EditProduct;

namespace ProductService.Functions
{
    public class ProductsFunction
    {
        private readonly ILogger<ProductsFunction> _logger;

        private const string OpenApiTag = "Product";

        private readonly IAuthenticationService authenticationService;
        private readonly IMediator mediator;

        public ProductsFunction(ILogger<ProductsFunction> log, IAuthenticationService authenticationService, IMediator mediator)
        {
            _logger = log;
            this.authenticationService = authenticationService;
            this.mediator = mediator;
        }

        [FunctionName("AddProduct")]
        [OpenApiOperation(operationId: "AddProduct", tags: OpenApiTag)]
        [OpenApiSecurity("BearerAuth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "Authorization")]
        [OpenApiRequestBody("application/json", typeof(AddProductCommand))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(AddProductResult), Description = "Adds new product")]
        public async Task<IActionResult> AddProduct(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/products")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request for Add Product");
            var authResult = this.authenticationService.ValidateToken(req);
            if (!authResult.IsValid)
            {
                _logger.LogInformation("User unauthorized");
                return new UnauthorizedResult(); // No authentication info.
            }
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<AddProductCommand>(requestBody);
            var result = await mediator.SendAsync<AddProductCommand, CustomResponse<AddProductResult>>(data);

            return result.GetResponse();
        }

        [FunctionName("GetProduct")]
        [OpenApiOperation(operationId: "GetProduct", tags: OpenApiTag)]
        [OpenApiSecurity("BearerAuth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "Authorization")]
        [OpenApiParameter("productId", Type = typeof(int), Visibility = OpenApiVisibilityType.Important, Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(GetProductResult), Description = "Gets existing product details")]
        public async Task<IActionResult> GetProduct(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/products/{productId:int}")] HttpRequest req, int productId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request for Get Product");
            var authResult = this.authenticationService.ValidateToken(req);
            if (!authResult.IsValid)
            {
                _logger.LogInformation("User unauthorized");
                return new UnauthorizedResult(); // No authentication info.
            }
            var result = await mediator.SendAsync<GetProductQuery, CustomResponse<GetProductResult>>(new GetProductQuery(productId));

            return result.GetResponse();
        }

        [FunctionName("GetProducts")]
        [OpenApiOperation(operationId: "GetProducts", tags: OpenApiTag)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(GetProductsResult), Description = "Gets list of products")]
        public async Task<IActionResult> GetProducts(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/products")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request for Get Products");
           
            var result = await mediator.SendAsync<GetProductsQuery, CustomResponse<GetProductsResult>>(new GetProductsQuery());

            return result.GetResponse();
        }

        [FunctionName("EditProduct")]
        [OpenApiOperation(operationId: "EditProduct", tags: OpenApiTag)]
        [OpenApiSecurity("BearerAuth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "Authorization")]
        [OpenApiParameter("productId", Type = typeof(int), Visibility = OpenApiVisibilityType.Important, Required = true)]
        [OpenApiRequestBody("application/json", typeof(ProductDetailDto))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(EditProductResult), Description = "Updates existing product details")]
        public async Task<IActionResult> EditProduct(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "v1/products/{productId:int}")] HttpRequest req, int productId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request for Edit Product");
            var authResult = this.authenticationService.ValidateToken(req);
            if (!authResult.IsValid)
            {
                _logger.LogInformation("User unauthorized");
                return new UnauthorizedResult(); // No authentication info.
            }
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<ProductDetailDto>(requestBody);
            var result = await mediator.SendAsync<EditProductCommand, CustomResponse<EditProductResult>>(new EditProductCommand(productId, data));

            return result.GetResponse();
        }
    }
}

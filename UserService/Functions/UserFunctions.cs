using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using UserService.Common;
using UserService.Data;
using UserService.Domain.AddUser;
using UserService.Services;

namespace UserService.Functions
{
    public class UserFunctions
    {
        private readonly ILogger<UserFunctions> _logger;

        private const string OpenApiTag = "User";

        private readonly UserContext dbContext;
        private readonly IAuthenticationService authenticationService;
        private readonly IMediator mediator;


        public UserFunctions(ILogger<UserFunctions> log, UserContext dbContext, IAuthenticationService authenticationService, IMediator mediator)
        {
            _logger = log;
            this.dbContext = dbContext;
            this.authenticationService = authenticationService;
            this.mediator = mediator;
            this.mediator.RegisterHandlers(Assembly.GetExecutingAssembly());
        }

        [FunctionName("AddUser")]
        [OpenApiOperation(operationId: "AddUser", tags: OpenApiTag)]
        [OpenApiSecurity("BearerAuth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "Authorization")]
        [OpenApiRequestBody("application/json", typeof(AddUserCommand))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(AddUserResult), Description = "The OK response")]
        public async Task<IActionResult> AddUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/users")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request for Add User");
            var authResult = this.authenticationService.ValidateToken(req);
            if (!authResult.IsValid)
            {
                _logger.LogInformation("User unauthorized");
                return new UnauthorizedResult(); // No authentication info.
            }
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<AddUserCommand>(requestBody);
            var result = mediator.Send<AddUserCommand, CustomResponse<AddUserResult>>(data);
            //var validator = new SaveClientValidator();
            //var validationResult = validator.Validate(data);
            //if (!validationResult.IsValid)
            //{
            //    return new BadRequestObjectResult(validationResult.Errors.Select(x => new { x.PropertyName, x.ErrorMessage }));
            //}
            //var handler = new SaveClientHandler();
            //var result = await handler.AddClient(this.dbContext, data);

            //if (result.Id == -1)
            //{
            //    return new BadRequestObjectResult("User already exists with same Name/Contact No");
            //}

            // create another service which manages response and returns http status code.
            return new OkObjectResult(result);
        }
    }
}


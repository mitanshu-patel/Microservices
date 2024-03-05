using System.IO;
using System.Net;
using System.Reflection;
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
using UserService.Common;
using UserService.Data;
using UserService.Domain.AddUser;
using UserService.Domain.AuthenticateUser;
using UserService.Services;

namespace UserService.Functions
{
    public class UserFunctions
    {
        private readonly ILogger<UserFunctions> _logger;

        private const string OpenApiTag = "User";

        private readonly IAuthenticationService authenticationService;
        private readonly IMediator mediator;


        public UserFunctions(ILogger<UserFunctions> log, IAuthenticationService authenticationService, IMediator mediator)
        {
            _logger = log;
            this.authenticationService = authenticationService;
            this.mediator = mediator;
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
            var result = await mediator.SendAsync<AddUserCommand, CustomResponse<AddUserResult>>(data);
            
            return result.GetResponse();
        }

        [FunctionName("AuthenticateUser")]
        [OpenApiOperation(operationId: "AuthenticateUser", tags: OpenApiTag)]
        [OpenApiRequestBody("application/json", typeof(AuthenticateUserCommand))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(AuthenticateUserResult), Description = "The OK response")]
        public async Task<IActionResult> AuthenticateUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/users/authenticate")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request for Authenticate User");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<AuthenticateUserCommand>(requestBody);
            var result = await mediator.SendAsync<AuthenticateUserCommand, CustomResponse<AuthenticateUserResult>>(data);

            return result.GetResponse();
        }
    }
}


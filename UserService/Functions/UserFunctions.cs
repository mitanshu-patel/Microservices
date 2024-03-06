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
using UserService.Domain.Common.DTOs;
using UserService.Domain.EditUser;
using UserService.Domain.GetUser;
using UserService.Domain.GetUsers;
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
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(AddUserResult), Description = "Adds new user")]
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

        [FunctionName("GetUser")]
        [OpenApiOperation(operationId: "GetUser", tags: OpenApiTag)]
        [OpenApiSecurity("BearerAuth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "Authorization")]
        [OpenApiParameter("userId", Type = typeof(int), Visibility = OpenApiVisibilityType.Important, Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(GetUserResult), Description = "Gets existing user details")]
        public async Task<IActionResult> GetUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/users/{userId:int}")] HttpRequest req, int userId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request for Get User");
            var authResult = this.authenticationService.ValidateToken(req);
            if (!authResult.IsValid)
            {
                _logger.LogInformation("User unauthorized");
                return new UnauthorizedResult(); // No authentication info.
            }
            var result = await mediator.SendAsync<GetUserQuery, CustomResponse<GetUserResult>>(new GetUserQuery(userId));

            return result.GetResponse();
        }

        [FunctionName("GetUsers")]
        [OpenApiOperation(operationId: "GetUsers", tags: OpenApiTag)]
        [OpenApiSecurity("BearerAuth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "Authorization")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(GetUsersResult), Description = "Gets list of users")]
        public async Task<IActionResult> GetUsers(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/users")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request for Get Users");
            var authResult = this.authenticationService.ValidateToken(req);
            if (!authResult.IsValid)
            {
                _logger.LogInformation("User unauthorized");
                return new UnauthorizedResult(); // No authentication info.
            }
            var result = await mediator.SendAsync<GetUsersQuery, CustomResponse<GetUsersResult>>(new GetUsersQuery());

            return result.GetResponse();
        }

        [FunctionName("EditUser")]
        [OpenApiOperation(operationId: "EditUser", tags: OpenApiTag)]
        [OpenApiSecurity("BearerAuth", SecuritySchemeType.ApiKey, In = OpenApiSecurityLocationType.Header, Name = "Authorization")]
        [OpenApiParameter("userId", Type = typeof(int), Visibility = OpenApiVisibilityType.Important, Required = true)]
        [OpenApiRequestBody("application/json", typeof(UserDetailDto))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(EditUserResult), Description = "Updates existing user details")]
        public async Task<IActionResult> EditUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "v1/users/{userId:int}")] HttpRequest req, int userId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request for Edit User");
            var authResult = this.authenticationService.ValidateToken(req);
            if (!authResult.IsValid)
            {
                _logger.LogInformation("User unauthorized");
                return new UnauthorizedResult(); // No authentication info.
            }
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<UserDetailDto>(requestBody);
            var result = await mediator.SendAsync<EditUserCommand, CustomResponse<EditUserResult>>(new EditUserCommand(userId, data));

            return result.GetResponse();
        }

        [FunctionName("AuthenticateUser")]
        [OpenApiOperation(operationId: "AuthenticateUser", tags: OpenApiTag)]
        [OpenApiRequestBody("application/json", typeof(AuthenticateUserCommand))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(AuthenticateUserResult), Description = "Authenticates existing user")]
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


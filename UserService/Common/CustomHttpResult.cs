using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace UserService.Common
{
    public static class CustomHttpResult
    {
        public static IActionResult GetResponse<T>(this CustomResponse<T> customResponse)
        {
            if(customResponse.ResponseCode == System.Net.HttpStatusCode.BadRequest)
            {
                return new BadRequestObjectResult(customResponse.ErrorMessage);
            }
            else if(customResponse.ResponseCode == System.Net.HttpStatusCode.NotFound)
            {
                return new NotFoundObjectResult(customResponse.ErrorMessage);
            }
            else if(customResponse.ResponseCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return new UnauthorizedObjectResult(customResponse.ErrorMessage);
            }
            else if(customResponse.ResponseCode == System.Net.HttpStatusCode.Forbidden)
            {
                return new ForbidResult(customResponse.ErrorMessage);
            }
            else if (customResponse.ResponseCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return new InternalServerErrorResult();
            }
            return new OkObjectResult(customResponse.Data);
        }
    }
}

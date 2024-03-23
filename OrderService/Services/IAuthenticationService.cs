using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Common;

namespace OrderService.Services
{
    public interface IAuthenticationService
    {
        void AddToken(string user, string token);

        JWTResult ValidateToken(HttpRequest request);
    }
}

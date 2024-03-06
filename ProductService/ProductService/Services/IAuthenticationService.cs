using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductService.Common;

namespace ProductService.Services
{
    public interface IAuthenticationService
    {
        string IssueJWT(string user);

        JWTResult ValidateToken(HttpRequest request);
    }
}

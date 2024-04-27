using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Domain.AuthenticateUser
{
    public record AuthenticateUserResult(string Token, string UserName, string Email, int UserId = 0);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Domain.GetUser
{
    public record GetUserResult(int UserId, string Email, string Name, string MobileNumber);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Messages
{
    public record UserDetailUpdated(int UserId, string Name, string Email, string MobileNo);
}

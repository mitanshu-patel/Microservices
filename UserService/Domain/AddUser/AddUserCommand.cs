using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Domain.AddUser
{
    public record AddUserCommand(string Name, string Email, string Password, string Mobile);
}

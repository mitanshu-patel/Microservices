using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Common.DTOs;

namespace UserService.Domain.GetUsers
{
    public record GetUsersResult(List<UserDetailDto> Users);
}

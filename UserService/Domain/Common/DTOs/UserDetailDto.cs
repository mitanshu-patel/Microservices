using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Domain.Common.DTOs
{
    public record UserDetailDto(int Id, string Name, string Email, string MobileNumber);
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Common;
using UserService.Data;
using UserService.Domain.Common.DTOs;

namespace UserService.Domain.GetUsers
{
    public class GetUsersHandler : IHandler<GetUsersQuery, CustomResponse<GetUsersResult>>
    {
        private readonly UserContext dbContext;
        public GetUsersHandler(UserContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<CustomResponse<GetUsersResult>> Handle(GetUsersQuery command)
        {
            var users = await this.dbContext.Users.Select(t => new UserDetailDto(t.Id, t.Name, t.Email, t.MobileNo))
                                                   .ToListAsync();

            return CustomHttpResult.Ok(new GetUsersResult(users));
        }
    }
}

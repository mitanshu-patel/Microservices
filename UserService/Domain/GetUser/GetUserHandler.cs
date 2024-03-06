using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Common;
using UserService.Data;

namespace UserService.Domain.GetUser
{
    public class GetUserHandler : IHandler<GetUserQuery, CustomResponse<GetUserResult>>
    {
        private readonly UserContext dbContext;
        public GetUserHandler(UserContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<CustomResponse<GetUserResult>> Handle(GetUserQuery command)
        {
            var userDetail = await this.dbContext.Users.Where(t => t.Id == command.UserId)
                                                    .Select(x => new GetUserResult(x.Id, x.Email, x.Name, x.MobileNo))
                                                    .FirstOrDefaultAsync();

            if(userDetail == null)
            {
                return CustomHttpResult.NotFound<GetUserResult>("User doesn't exist");
            }

            return CustomHttpResult.Ok(userDetail);
        }
    }
}

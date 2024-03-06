using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Common;
using UserService.Data;

namespace UserService.Domain.EditUser
{
    public class EditUserHandler : IHandler<EditUserCommand, CustomResponse<EditUserResult>>
    {
        private readonly UserContext dbContext;
        public EditUserHandler(UserContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<CustomResponse<EditUserResult>> Handle(EditUserCommand command)
        {
            var userDetail = await this.dbContext.Users.FindAsync(command.UserId);
            if(userDetail == null)
            {
                return CustomHttpResult.NotFound<EditUserResult>("User doesn't exist");
            }

            userDetail.Email = command.UserDetails.Email;
            userDetail.Name = command.UserDetails.Name;
            userDetail.MobileNo = command.UserDetails.MobileNumber;

            this.dbContext.Entry(userDetail).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await this.dbContext.SaveChangesAsync();

            return CustomHttpResult.Ok<EditUserResult>(new EditUserResult());
        }
    }
}

using Microsoft.EntityFrameworkCore;
using OrderService.Common;
using OrderService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Messaging.UserDetailUpdated
{
    public class UserDetailUpdatedHandler : IHandler<UserDetailUpdatedCommand, CustomResponse<UserDetailUpdatedResult>>
    {
        private readonly OrderContext dbContext;

        public UserDetailUpdatedHandler(OrderContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<CustomResponse<UserDetailUpdatedResult>> Handle(UserDetailUpdatedCommand command)
        {
            var userDetail = await this.dbContext.RemoteUsers.FindAsync(command.userDetailUpdated.UserId);
            if(userDetail == null)
            {
                userDetail = new Data.Entities.RemoteUser
                {
                    Email = command.userDetailUpdated.Email,
                    Name = command.userDetailUpdated.Name,
                    MobileNo = command.userDetailUpdated.MobileNo,
                    Id = command.userDetailUpdated.UserId,
                };

                this.dbContext.RemoteUsers.Add(userDetail);
            }
            else
            {
                userDetail.Email = command.userDetailUpdated.Email;
                userDetail.Name = command.userDetailUpdated.Name;
                userDetail.MobileNo = command.userDetailUpdated.MobileNo;
                userDetail.Id = command.userDetailUpdated.UserId;
                this.dbContext.Entry(userDetail).State = EntityState.Modified;
            }

            await this.dbContext.SaveChangesAsync();

            return CustomHttpResult.Ok(new UserDetailUpdatedResult());
        }
    }
}

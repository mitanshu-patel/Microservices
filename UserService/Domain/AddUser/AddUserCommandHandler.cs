using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Common;
using UserService.Data;
using UserService.Data.Entities;
using UserService.Messages;
using UserService.Services;

namespace UserService.Domain.AddUser
{
    public class AddUserCommandHandler : IHandler<AddUserCommand, CustomResponse<AddUserResult>>
    {
        private readonly UserContext dbContext;
        private readonly IMessageDeliveryService messageDeliveryService;
        public AddUserCommandHandler(UserContext dbContext, IMessageDeliveryService messageDeliveryService)
        {
            this.dbContext = dbContext;
            this.messageDeliveryService = messageDeliveryService;
        }

        public async Task<CustomResponse<AddUserResult>> Handle(AddUserCommand command)
        {
            var isUserExist = await this.IsUserExist(command.Email);

            if (isUserExist)
            {
                return CustomHttpResult.BadRequest<AddUserResult>("User Already Exists");
            }

            var user = new User
            {
                Name = command.Name,
                Password = command.Password.ComputeSHA256Hash(),
                Email = command.Email,
                MobileNo = command.Mobile,
            };

            this.dbContext.Users.Add(user);
            await this.dbContext.SaveChangesAsync();

            //await this.messageDeliveryService.PublishMessageAsync(new UserDetailUpdated(user.Id, user.Name, user.Email, user.MobileNo), "userserviceevents");
            return CustomHttpResult.Ok(new AddUserResult(user.Id));
        }

        public async Task<bool> IsUserExist(string email)
        {
            return await this.dbContext.Users.AnyAsync(t => t.Email == email);
        }
    }
}

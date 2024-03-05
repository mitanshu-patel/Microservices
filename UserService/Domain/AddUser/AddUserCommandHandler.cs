using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Common;
using UserService.Data;
using UserService.Data.Entities;

namespace UserService.Domain.AddUser
{
    public class AddUserCommandHandler : IHandler<AddUserCommand, CustomResponse<AddUserResult>>
    {
        private readonly UserContext dbContext;
        private readonly ILogger<AddUserCommandHandler> logger;
        public AddUserCommandHandler(UserContext dbContext, ILogger<AddUserCommandHandler> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<CustomResponse<AddUserResult>> Handle(AddUserCommand command)
        {
            var user = new User
            {
                Name = command.Name,
                Password = command.Password,
                Email = command.Email,
            };

            this.dbContext.Users.Add(user);
            await this.dbContext.SaveChangesAsync();

            var result = new CustomResponse<AddUserResult>
            {
                Data = new AddUserResult(user.Id),
                ResponseCode = System.Net.HttpStatusCode.OK,
            };

            return result;
        }
    }
}

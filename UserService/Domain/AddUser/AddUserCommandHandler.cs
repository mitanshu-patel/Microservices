using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Common;
using UserService.Data;

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

        public CustomResponse<AddUserResult> Handle(AddUserCommand command)
        {
            throw new NotImplementedException();
        }
    }
}

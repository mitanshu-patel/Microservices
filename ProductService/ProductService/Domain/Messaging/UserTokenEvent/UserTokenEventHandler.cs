using ProductService.Common;
using ProductService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Messaging.UserTokenEvent
{
    public class UserTokenEventHandler : IHandler<UserTokenEventCommand, CustomResponse<UserTokenEventResult>>
    {
        private readonly IAuthenticationService authenticationService;

        public UserTokenEventHandler(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        public async Task<CustomResponse<UserTokenEventResult>> Handle(UserTokenEventCommand command)
        {
            this.authenticationService.AddToken(command.UserTokenUpdated.Email, command.UserTokenUpdated.Token);
            return CustomHttpResult.Ok(new UserTokenEventResult());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Messages;

namespace ProductService.Domain.Messaging.UserTokenEvent
{
    public record UserTokenEventCommand(UserTokenUpdated UserTokenUpdated);
}

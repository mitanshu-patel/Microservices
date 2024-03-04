using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Common
{
    // Updated IHandler interface
    public interface IHandler<TCommand, TResult> where TCommand : class
    {
        TResult Handle(TCommand command);
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Services
{
    public interface IMediator
    {
        void RegisterHandlers(Assembly assembly);
        TResult Send<TCommand, TResult>(TCommand command) where TCommand : class;
    }
}

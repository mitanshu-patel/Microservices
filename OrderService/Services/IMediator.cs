using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Services
{
    public interface IMediator
    {
        Task<TResult> SendAsync<TCommand, TResult>(TCommand command) where TCommand : class;
    }
}

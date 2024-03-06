// Updated Mediator implementation
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using ProductService.Common;
using ProductService.Services;

// Mediator
public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResult> SendAsync<TCommand, TResult>(TCommand command) where TCommand: class
    {
        var handler = _serviceProvider.GetRequiredService<IHandler<TCommand, TResult>>();
        return await handler.Handle(command);
    }
}


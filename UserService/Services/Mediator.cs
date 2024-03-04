// Updated Mediator implementation
using System.Reflection;
using System.Windows.Input;
using UserService.Common;
using UserService.Services;

public class Mediator : IMediator
{
    private readonly Dictionary<Type, MethodInfo> _handlers = new Dictionary<Type, MethodInfo>();

    public void RegisterHandlers(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            var interfaces = type.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IHandler<,>))
                {
                    var genericArguments = @interface.GetGenericArguments();
                    var commandType = genericArguments[0];
                    var resultType = genericArguments[1];
                    var methodInfo = type.GetMethod("Handle", new[] { commandType });
                    if (methodInfo != null)
                    {
                        _handlers[commandType] = methodInfo;
                    }
                }
            }
        }
    }

    public TResult Send<TCommand, TResult>(TCommand command) where TCommand : class
    {
        var commandType = typeof(TCommand);
        if (_handlers.TryGetValue(commandType, out var methodInfo))
        {
            var handlerInstance = Activator.CreateInstance(methodInfo.DeclaringType);
            return (TResult)methodInfo.Invoke(handlerInstance, new object[] { command });
        }
        else
        {
            throw new InvalidOperationException($"No handler registered for {commandType.Name}");
        }
    }
}

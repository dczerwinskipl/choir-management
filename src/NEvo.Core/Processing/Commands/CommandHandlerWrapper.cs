using Microsoft.Extensions.DependencyInjection;
using NEvo.Core;
using NEvo.Messaging;
using NEvo.Messaging.Commands;
using NEvo.Processing.Registering;
using System;

namespace NEvo.Processing.Commands;

public class CommandHandlerWrapper<THandler, TMessage> : IMessageHandlerWrapper
                                                            where THandler : ICommandHandler<TMessage> 
                                                            where TMessage : Command
{
    public MessageHandlerDescription Description { get; }
    private readonly IServiceProvider _serviceProvider;

    public CommandHandlerWrapper(MessageHandlerDescription description, IServiceProvider serviceProvider)
    {
        Description = description;
        _serviceProvider = serviceProvider;
    }

    public async Task<Try<object>> Handle(IMessage message)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();

            var handler = ActivatorUtilities.CreateInstance<THandler>(scope.ServiceProvider);
            var result = await handler.HandleAsync(Check.Null(message as TMessage));

            return
                result.Handle(
                    result => Try.Success<object>(result),
                    Try.Failure<object>
                );
                
        } 
        catch(Exception exception)
        {
            return Try.Failure<object>(exception);
        }
        
    }
}

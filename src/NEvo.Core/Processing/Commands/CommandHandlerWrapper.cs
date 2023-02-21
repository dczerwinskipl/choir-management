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

    public async Task<Either<MessageProcessingFailure, MessageProcessingSuccess<object>>> Handle(IMessage message)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();

            var handler = ActivatorUtilities.CreateInstance<THandler>(scope.ServiceProvider);
            var result = await handler.HandleAsync(Check.Null(message as TMessage));

            return
                result.Handle(
                    result => Either.Right<MessageProcessingFailure, MessageProcessingSuccess<object>>(new MessageProcessingSuccess<object>(Description.HandlerType, result)),
                    exception => Either.Left<MessageProcessingFailure, MessageProcessingSuccess<object>>(new MessageProcessingFailure(Description.HandlerType, exception))
                );
                
        } 
        catch(Exception exception)
        {
            return Either.Left<MessageProcessingFailure, MessageProcessingSuccess<object>>(new MessageProcessingFailure(Description.HandlerType, exception));
        }
        
    }
}

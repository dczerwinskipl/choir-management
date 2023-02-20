using Microsoft.Extensions.DependencyInjection;
using NEvo.Messaging;
using NEvo.Messaging.Commands;
using NEvo.Processing.Commands;
using NEvo.Processing.Registering;

namespace NEvo.Core.Processing.Commands;

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

    public async Task<Either<MessageProcessingFailure, MessageProcessingSuccess>> Handle(IMessage message)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();

            var handler = ActivatorUtilities.CreateInstance<THandler>(scope.ServiceProvider);
            await handler.HandleAsync(Check.Null(message as TMessage));

            return Either.Right<MessageProcessingFailure, MessageProcessingSuccess>(new MessageProcessingSuccess(Description.HandlerType));
        } 
        catch(Exception exception)
        {
            return Either.Left<MessageProcessingFailure, MessageProcessingSuccess>(new MessageProcessingFailure(Description.HandlerType, exception));
        }
        
    }
}

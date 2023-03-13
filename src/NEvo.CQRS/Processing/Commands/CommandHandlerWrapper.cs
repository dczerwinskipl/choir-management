using Microsoft.Extensions.DependencyInjection;
using NEvo.Core;
using NEvo.Monads;
using NEvo.Messaging;
using NEvo.Messaging.Commands;
using NEvo.Processing.Registering;

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

    public async Task<Either<Exception, object?>> Handle(IMessage message)
    {
        return await Try.OfAsync(async () =>
        {
            using var scope = _serviceProvider.CreateScope();

            var handler = ActivatorUtilities.CreateInstance<THandler>(scope.ServiceProvider);
            var result = await handler.HandleAsync(Check.Null(message as TMessage));

            return result.Cast<object?>();
        });
        
    }
}

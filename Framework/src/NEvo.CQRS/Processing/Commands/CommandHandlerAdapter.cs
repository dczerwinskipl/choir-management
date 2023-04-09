using Microsoft.Extensions.DependencyInjection;
using NEvo.Core;
using NEvo.CQRS.Messaging;
using NEvo.CQRS.Messaging.Commands;
using NEvo.CQRS.Processing.Registering;
using NEvo.Monads;

namespace NEvo.CQRS.Processing.Commands;

public class CommandHandlerAdapter<THandler, TMessage> : IMessageHandlerAdapter
                                                            where THandler : ICommandHandler<TMessage>
                                                            where TMessage : Command
{
    public MessageHandlerDescription Description { get; }
    private readonly IServiceProvider _serviceProvider;

    public CommandHandlerAdapter(MessageHandlerDescription description, IServiceProvider serviceProvider)
    {
        Description = description;
        _serviceProvider = serviceProvider;
    }

    public async Task<Either<Exception, object?>> HandleAsync(IMessage message)
    {
        return await Try.OfAsync(async () =>
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            var handler = ActivatorUtilities.CreateInstance<THandler>(scope.ServiceProvider);
            var result = await handler.HandleAsync(Check.Null(message as TMessage));

            return result.Cast<object?>();
        });

    }
}

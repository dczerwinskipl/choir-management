using Microsoft.Extensions.DependencyInjection;
using NEvo.Core;
using NEvo.CQRS.Messaging;
using NEvo.CQRS.Messaging.Queries;
using NEvo.CQRS.Processing.Registering;
using NEvo.Monads;

namespace NEvo.CQRS.Processing.Queries;

public class QueryHandlerAdapter<THandler, TMessage, TResult> : IMessageHandlerAdapter
                                                            where THandler : IQueryHandler<TMessage, TResult>
                                                            where TMessage : Query<TResult>
{
    public MessageHandlerDescription Description { get; }
    private readonly IServiceProvider _serviceProvider;

    public QueryHandlerAdapter(MessageHandlerDescription description, IServiceProvider serviceProvider)
    {
        Description = description;
        _serviceProvider = serviceProvider;
    }

    public async Task<Either<Exception, object?>> HandleAsync(IMessage message)
    {
        return await Try.OfAsync(async () =>
        {
            using var scope = _serviceProvider.CreateScope();

            var handler = ActivatorUtilities.CreateInstance<THandler>(scope.ServiceProvider);
            var result = await handler.HandleAsync(Check.Null(message as TMessage));

            return result;
        });

    }
}

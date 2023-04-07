using NEvo.Core;
using NEvo.Core.Reflection;
using NEvo.CQRS.Messaging.Commands;
using NEvo.CQRS.Messaging.Events;
using NEvo.CQRS.Messaging.Queries;
using NEvo.CQRS.Routing;
using NEvo.Monads;

namespace NEvo.CQRS.Messaging;

/// <summary>
/// Message bus that use internal infrastructure to process messages in synchronous way
/// </summary>
public class MessageBus : IMessageBus
{
    private readonly IMessageRouter _router;
    private readonly ITypeResolver _typeResolver;

    public MessageBus(IMessageRouter router, ITypeResolver typeResolver)
    {
        _router = Check.Null(router);
        _typeResolver = Check.Null(typeResolver);
    }

    public Task<Either<Exception, Unit>> DispatchAsync<TCommand>(TCommand command) where TCommand : Command
        => _router.ForMessage(command).DispatchMessageAsync<TCommand, Unit>(
            new MessageEnvelope<TCommand>(Guid.NewGuid(), command, _typeResolver.GetName(typeof(TCommand)))
        );

    public Task<Either<Exception, TResult>> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : Query<TResult>
        => _router.ForMessage(query).DispatchMessageAsync<TQuery, TResult>(
            new MessageEnvelope<TQuery>(Guid.NewGuid(), query, _typeResolver.GetName(typeof(TQuery)))
        );

    public Task PublishAsync<TEvent>(TEvent @event) where TEvent : Event
        => _router.ForMessage(@event).PublishMessageAsync(
            new MessageEnvelope<TEvent>(Guid.NewGuid(), @event, _typeResolver.GetName(@event.GetType()), @event.Source?.ToString())
        );
}

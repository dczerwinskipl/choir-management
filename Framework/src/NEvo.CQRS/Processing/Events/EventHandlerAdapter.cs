﻿using Microsoft.Extensions.DependencyInjection;
using NEvo.Core;
using NEvo.CQRS.Messaging;
using NEvo.CQRS.Messaging.Events;
using NEvo.CQRS.Processing.Registering;
using NEvo.Monads;

namespace NEvo.CQRS.Processing.Events;

public class EventHandlerAdapter<THandler, TMessage> : IMessageHandlerAdapter
                                                            where THandler : IEventHandler<TMessage>
                                                            where TMessage : Event
{
    public MessageHandlerDescription Description { get; }
    private readonly IServiceProvider _serviceProvider;

    public EventHandlerAdapter(MessageHandlerDescription description, IServiceProvider serviceProvider)
    {
        Description = description;
        _serviceProvider = serviceProvider;
    }

    public async Task<Either<Exception, object?>> HandleAsync(IMessage message)
    {
        return (await Try.OfAsync(async () =>
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            var handler = ActivatorUtilities.CreateInstance<THandler>(scope.ServiceProvider);
            await handler.HandleAsync(Check.Null(message as TMessage));
        })).Cast<object?>();
    }
}
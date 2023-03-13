using NEvo.Core;
using NEvo.Monads;
using NEvo.Messaging.Commands;
using NEvo.Messaging.Events;
using NEvo.Messaging.Queries;
using NEvo.Processing;
using Newtonsoft.Json;

namespace NEvo.Messaging;

/// <summary>
/// Message bus that use internal infrastructure to process messages in synchronous way
/// </summary>
public class InternalMessageBus : IMessageBus
{
    private readonly IMessageProcessor _messageProcessor;

    public InternalMessageBus(IMessageProcessor messageProcessor)
    {
        _messageProcessor = Check.Null(messageProcessor);
    }

    public async Task<Either<Exception, Unit>> DispatchAsync(Command command) =>
        await _messageProcessor.ProcessAsync<Command, Unit>(command)
        .Map(
            Either.Success,
            failure => Either.Failure(failure.Count() > 1 ? new AggregateException(failure.Select(s => s.Exception)) : failure.Single().Exception)
         );

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : Event =>
        (await _messageProcessor.ProcessAsync(@event))
        .OnFailure(failures => throw new AggregateException(failures.Select(s => s.Exception)));

    public async Task<Either<Exception, TResult>> DispatchAsync<TResult>(Query<TResult> query) =>
        await _messageProcessor.ProcessAsync<Query<TResult>, TResult>(query)
        .Map(
            Either.Success,
            failure => Either.Failure<TResult>(failure.Count() > 1 ? new AggregateException(failure.Select(s => s.Exception)) : failure.Single().Exception)
         );
}


public class ExternalMessageBus : IMessageBus
{
    private readonly IMessageProcessor _messageProcessor;
    private readonly Publishing.IMessagePublisher _messageBus;

    public ExternalMessageBus(IMessageProcessor messageProcessor, Publishing.IMessagePublisher messageBus)
    {
        _messageProcessor = Check.Null(messageProcessor);
        _messageBus = messageBus;
    }

    public async Task<Either<Exception, Unit>> DispatchAsync(Command command) =>
        await _messageProcessor.ProcessAsync<Command, Unit>(command)
        .Map(
            Either.Success,
            failure => Either.Failure(failure.Count() > 1 ? new AggregateException(failure.Select(s => s.Exception)) : failure.Single().Exception)
         );

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : Event
    {
        var payload = JsonConvert.SerializeObject(@event);

        await _messageBus.PublishAsync(new MessageEnvelope<TEvent>(Guid.NewGuid(), @event, $"{@event.GetType().FullName}, {@event.GetType().Assembly.GetName().Name}" /* todo: another service for messageType */), @event.Source?.ToString() ?? string.Empty);
    }

    public async Task<Either<Exception, TResult>> DispatchAsync<TResult>(Query<TResult> query) =>
        await _messageProcessor.ProcessAsync<Query<TResult>, TResult>(query)
        .Map(
            Either.Success,
            failure => Either.Failure<TResult>(failure.Count() > 1 ? new AggregateException(failure.Select(s => s.Exception)) : failure.Single().Exception)
         );
}

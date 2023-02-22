using NEvo.Core;
using NEvo.Messaging;
using NEvo.Messaging.Commands;
using NEvo.Messaging.Events;
using NEvo.Messaging.Queries;

namespace NEvo.Processing;

/// <summary>
/// Message bus that use internal infrastructure to process messages in synchronous way
/// </summary>
public class SynchronousMessageBus : IMessageBus
{
    private readonly IMessageProcessor _messageProcessor;

    public SynchronousMessageBus(IMessageProcessor messageProcessor)
    {
        _messageProcessor = Check.Null(messageProcessor);
    }

    public async Task<Try<Unit>> DispatchAsync(Command command) => 
        (await _messageProcessor.ProcessAsync<Command, Unit>(command))
        .Handle(
            _ => Try.Success(), 
            failure => Try.Failure(failure.Count() > 1 ? new AggregateException(failure.Select(s => s.Exception)) : failure.Single().Exception)
         );

    public async Task PublishAsync(Event @event) =>
        (await _messageProcessor.ProcessAsync(@event))
        .OnFailure(failures => throw new AggregateException(failures.Select(s => s.Exception)));

    public async Task<Try<TResult>> DispatchAsync<TResult>(Query<TResult> query) => 
        (await _messageProcessor.ProcessAsync<Query<TResult>, TResult>(query))
        .Handle(
            result => Try.Success(result),
            failure => Try.Failure<TResult>(failure.Count() > 1 ? new AggregateException(failure.Select(s => s.Exception)) : failure.Single().Exception)
         );
}

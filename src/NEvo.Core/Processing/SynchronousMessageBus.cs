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

    public async Task<Either<Exception, Unit>> DispatchAsync(Command command) => 
        (await _messageProcessor.ProcessAsync<Command, Unit>(command))
        .Handle(
            _ => Either.Right(), 
            failure => Either.Left(new AggregateException(failure.Select(s => s.Exception)))
         );

    public async Task<Either<Exception, Unit>> PublishAsync(Event @event) => 
        (await _messageProcessor.ProcessAsync<Event, Unit>(@event))
        .Handle(
            _ => Either.Right(),
            failure => Either.Left(new AggregateException(failure.Select(s => s.Exception)))
         );

    public async Task<Either<Exception, TResult>> DispatchAsync<TResult>(Query<TResult> query) => 
        (await _messageProcessor.ProcessAsync<Query<TResult>, TResult>(query))
        .Handle(
            result => Either.Right(result),
            failure => Either.Left<TResult>(new AggregateException(failure.Select(s => s.Exception)))
         );
}

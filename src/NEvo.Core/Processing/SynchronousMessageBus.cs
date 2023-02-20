using NEvo.Messaging;
using NEvo.Messaging.Commands;
using NEvo.Messaging.Events;
using NEvo.Messaging.Queries;

namespace NEvo.Core.Processing;

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

    public async Task DispatchAsync(Command command) => (await _messageProcessor.ProcessAsync(command))
                                                            .OnFailure(failures => throw ThrowProcessingException(failures));

    public async Task PublishAsync(Event @event) => (await _messageProcessor.ProcessAsync(@event))
                                                            .OnFailure(failures => throw ThrowProcessingException(failures));

    public async Task<TResult> DispatchAsync<TResult>(Query<TResult> query) => (await _messageProcessor.ProcessAsync(query))
                                                                                    .Handle(
                                                                                        result => ((IMessageProcessingResult<TResult>)result).Result,
                                                                                        failures => { throw ThrowProcessingException(failures); }
                                                                                    );
    private static Exception ThrowProcessingException(IMessageProcessingFailures processingFailures) => new AggregateException(processingFailures.Select(f => f.Exception));  //TODO: add custom exception

}

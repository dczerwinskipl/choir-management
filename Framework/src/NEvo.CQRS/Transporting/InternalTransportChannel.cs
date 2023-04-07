using NEvo.Core;
using NEvo.CQRS.Messaging;
using NEvo.CQRS.Processing;
using NEvo.Monads;

namespace NEvo.CQRS.Transporting;

/// <summary>
/// Class that represents internal transport channel that use message processor directly
/// 
/// ATM this implementation ignores settings from InternalTransportChannelDescription
/// </summary>
public class InternalTransportChannel : ITransportChannel
{
    private readonly IMessageProcessor _messageProcessor;
    private readonly TransportChannelDescription.InternalTransportChannelDescription _channelDescription;

    public InternalTransportChannel(IMessageProcessor messageProcessor, TransportChannelDescription.InternalTransportChannelDescription channelDescription)
    {
        _messageProcessor = Check.Null(messageProcessor);
        _channelDescription = Check.Null(channelDescription);
    }

    public async Task<Either<Exception, TResult>> DispatchMessageAsync<TMessage, TResult>(MessageEnvelope<TMessage> messsageEnvelope) where TMessage : IMessage<TResult>
        => await _messageProcessor
                     .ProcessAsync<TMessage, TResult>(messsageEnvelope.Payload)
                     .BindLeft(ToException);

    public async Task<Either<Exception, Unit>> PublishMessageAsync<TMessage>(MessageEnvelope<TMessage> messsageEnvelope) where TMessage : IMessage<Unit>
        // todo: _channelDescription.UseAsync
        => await _messageProcessor
                    .ProcessAsync(messsageEnvelope.Payload)
                    .BindLeft(ToException);

    private static Exception ToException(IMessageProcessingFailures failures)
        => failures.Count() > 1 ? new AggregateException(failures.Select(f => f.Exception)) : failures.Single().Exception;

}

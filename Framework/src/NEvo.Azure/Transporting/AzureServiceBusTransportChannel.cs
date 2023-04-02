using NEvo.Azure.Publishing;
using NEvo.Core;
using NEvo.CQRS.Messaging;
using NEvo.CQRS.Transporting;
using NEvo.Monads;

namespace NEvo.Azure.Transporting;

/// <summary>
/// Adapter to user Azure Message Bus as transport channel
/// </summary>
public class AzureServiceBusTransportChannel : ITransportChannel
{
    private string _topicName;
    private IAzureServiceBusMessagePublisher _azureServiceBusMessagePublisher;

    public AzureServiceBusTransportChannel(IAzureServiceBusMessagePublisher azureServiceBusMessagePublisher, string topicName)
    {
        _topicName = topicName;
        _azureServiceBusMessagePublisher = azureServiceBusMessagePublisher;
    }

    public Task<Either<Exception, TResult>> DispatchMessageAsync<TMessage, TResult>(MessageEnvelope<TMessage> messageEnvelope) where TMessage : IMessage<TResult>
    {
        throw new NotImplementedException();
    }

    public async Task<Either<Exception, Unit>> PublishMessageAsync<TMessage>(MessageEnvelope<TMessage> messageEnvelope) where TMessage : IMessage<Unit>
        => await Try.OfAsync(async() => await _azureServiceBusMessagePublisher.PublishAsync(messageEnvelope, _topicName));
}

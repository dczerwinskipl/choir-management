using NEvo.CQRS.Messaging;

namespace NEvo.Publishing;

public interface IMessagePublisher
{
    void Publish<TMessage>(MessageEnvelope<TMessage> message) where TMessage : IMessage => PublishAsync(message).ConfigureAwait(false).GetAwaiter().GetResult();
    Task PublishAsync<TMessage>(MessageEnvelope<TMessage> messsageEnvelope) where TMessage : IMessage;
}

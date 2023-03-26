using NEvo.Messaging;

namespace NEvo.Publishing;

public interface IMessagePublisher
{
    void Publish<TMessage>(MessageEnvelope<TMessage> message, string partitionKey) where TMessage : IMessage => PublishAsync(message, partitionKey).ConfigureAwait(false).GetAwaiter().GetResult();
    Task PublishAsync<TMessage>(MessageEnvelope<TMessage> messsageEnvelope, string partitionKey /* move to DTO or sth*/) where TMessage : IMessage;
}

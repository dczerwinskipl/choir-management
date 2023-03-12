using NEvo.Messaging;

namespace NEvo.Messaging;

//TODO: do it in another way
public interface ITopicProvider
{
    public string TopicFor<TMessage>(MessageEnvelope<TMessage> messageEnvelope) where TMessage : IMessage;
}

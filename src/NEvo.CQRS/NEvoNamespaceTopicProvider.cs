using NEvo.Messaging;

namespace NEvo.Core;

public class NEvoNamespaceTopicProvider : ITopicProvider
{
    private const string Suffix = ".Messages";

    public NEvoNamespaceTopicProvider()
    {
    }

    public string TopicFor<TMessage>(MessageEnvelope<TMessage> messageEnvelope) where TMessage : IMessage
    {
        var messageNamespace = typeof(TMessage).Namespace;
        return messageNamespace.EndsWith(Suffix) ? messageNamespace.Substring(0, messageNamespace.Length - Suffix.Length) : messageNamespace;
    }
}
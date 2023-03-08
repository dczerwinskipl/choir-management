using NEvo.Messaging;

namespace NEvo.Core;

public class NEvoAppNameTopicProvider : ITopicProvider
{
    private readonly NEvoAppDetails _appDetails;

    public NEvoAppNameTopicProvider(INEvoAppDetailsProvider appDetailsProvider)
    {
        _appDetails = appDetailsProvider.GetAppDetails();
    }

    public string TopicFor<TMessage>(MessageEnvelope<TMessage> messageEnvelope) where TMessage : IMessage
    {
        return $"{_appDetails.AppName}"; //TODO: add -event -command -query sufix?
    }
}

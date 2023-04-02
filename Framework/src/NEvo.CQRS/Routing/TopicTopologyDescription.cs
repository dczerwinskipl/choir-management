namespace NEvo.CQRS.Routing;

public class TopicTopologyDescription : IChannelTopologyDescription
{
    public string ChannelType { get; set; }
    public string TopicName { get; set; }
}

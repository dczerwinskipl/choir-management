namespace NEvo.CQRS.Routing;

public interface IRoutingTopologyProvider
{
    EndpointTopologyDescription GetEndpointDescription(string endpointName);
    TopicTopologyDescription GetTopicDescription(string topicName);
}

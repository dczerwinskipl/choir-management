namespace NEvo.CQRS.Routing;

public class RoutingTopologyDescription
{
    public Dictionary<string, EndpointTopologyDescription> Endpoints { get; set; } = new();
    public Dictionary<string, TopicTopologyDescription> Topics { get; set; } = new();
}

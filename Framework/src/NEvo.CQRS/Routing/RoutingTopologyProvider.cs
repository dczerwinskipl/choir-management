using Microsoft.Extensions.Options;
using NEvo.Core;

namespace NEvo.CQRS.Routing;

public class RoutingTopologyProvider : IRoutingTopologyProvider
{
    private readonly Dictionary<string, EndpointTopologyDescription> _endpoints;
    private readonly Dictionary<string, TopicTopologyDescription> _topics;

    public RoutingTopologyProvider(IOptions<RoutingTopologyDescription> options)
    {
        _endpoints = Check.Null(options.Value?.Endpoints);
        _topics = Check.Null(options.Value?.Topics);
    }
    public EndpointTopologyDescription GetEndpointDescription(string endpointName)
        => _endpoints.TryGetValue(endpointName, out var description)
            ? description
            : throw new ArgumentException($"Endpoint description for {endpointName} not found.");

    public TopicTopologyDescription GetTopicDescription(string topicName)
        => _topics.TryGetValue(topicName, out var description)
            ? description
            : throw new ArgumentException($"Topic description for {topicName} not found.");
}
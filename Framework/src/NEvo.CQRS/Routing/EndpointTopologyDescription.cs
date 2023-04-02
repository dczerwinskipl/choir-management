namespace NEvo.CQRS.Routing;

public class EndpointTopologyDescription : IChannelTopologyDescription
{
    public string ChannelType { get; set; }
    public string Endpoint { get; set; }
}

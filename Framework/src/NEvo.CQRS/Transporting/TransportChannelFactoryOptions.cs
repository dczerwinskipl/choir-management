using NEvo.CQRS.Routing;
using static NEvo.CQRS.Transporting.TransportChannelDescription;

namespace NEvo.CQRS.Transporting;

public class TransportChannelFactoryOptions
{
    public ITransportChannelFactory<InternalTransportChannel, InternalTransportChannelDescription, InternalChannelTopologyDescription> InternalTransportChannelFactory { get; set; } = new InternalTransportChannelFactory();
    public List<ITransportChannelFactory<ITransportChannel, EndpointTransportChannelDescription, EndpointTopologyDescription>> EndpointTransportChannelFactories { get; set; } = new();
    public List<ITransportChannelFactory<ITransportChannel, MessagePublisherTransportChannelDescription, TopicTopologyDescription>> MessagePublisherTransportChannelFactories { get; set; } = new();
}

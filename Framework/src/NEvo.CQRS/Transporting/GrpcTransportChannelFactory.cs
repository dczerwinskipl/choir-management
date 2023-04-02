using NEvo.CQRS.Routing;
using static NEvo.CQRS.Transporting.TransportChannelDescription;

namespace NEvo.CQRS.Transporting;

public class GrpcTransportChannelFactory : ITransportChannelFactory<GrpcTransportChannel, EndpointTransportChannelDescription, EndpointTopologyDescription>
{
    public GrpcTransportChannel CreateChannel(IServiceProvider serviceProvider, EndpointTransportChannelDescription channelDescription, EndpointTopologyDescription endpointTopologyDescription)
    {
        throw new NotImplementedException();
    }
}

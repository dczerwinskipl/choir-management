using NEvo.CQRS.Routing;
using static NEvo.CQRS.Transporting.TransportChannelDescription;

namespace NEvo.CQRS.Transporting;

public class RestTransportChannelFactory : ITransportChannelFactory<RestTransportChannel, EndpointTransportChannelDescription, EndpointTopologyDescription>
{
    public RestTransportChannel CreateChannel(IServiceProvider serviceProvider, EndpointTransportChannelDescription channelDescription, EndpointTopologyDescription endpointTopologyDescription)
    {
        throw new NotImplementedException();
    }
}

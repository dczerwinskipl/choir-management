using Microsoft.Extensions.DependencyInjection;
using NEvo.CQRS.Routing;
using static NEvo.CQRS.Transporting.TransportChannelDescription;

namespace NEvo.CQRS.Transporting;

public class RestTransportChannelFactory : ITransportChannelFactory<RestTransportChannel, EndpointTransportChannelDescription, EndpointTopologyDescription>
{
    public RestTransportChannel CreateChannel(IServiceProvider serviceProvider, EndpointTransportChannelDescription channelDescription, EndpointTopologyDescription endpointTopologyDescription)
    {
        var restClient = ActivatorUtilities.CreateInstance<CQRSRestClient>(serviceProvider, new Uri(endpointTopologyDescription.Endpoint)) as ICQRSRestClient;
        return new RestTransportChannel(restClient);
    }
}

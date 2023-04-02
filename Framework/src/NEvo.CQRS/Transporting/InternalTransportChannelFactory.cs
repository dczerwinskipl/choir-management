using Microsoft.Extensions.DependencyInjection;
using NEvo.CQRS.Processing;
using NEvo.CQRS.Routing;
using static NEvo.CQRS.Transporting.TransportChannelDescription;

namespace NEvo.CQRS.Transporting;

public class InternalTransportChannelFactory : ITransportChannelFactory<InternalTransportChannel, InternalTransportChannelDescription, InternalChannelTopologyDescription>
{
    public InternalTransportChannel CreateChannel(IServiceProvider serviceProvider, InternalTransportChannelDescription channelDescription, InternalChannelTopologyDescription description)
    {
        return new InternalTransportChannel(serviceProvider.GetRequiredService<IMessageProcessor>(), channelDescription);
    }
}

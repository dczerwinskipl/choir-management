using NEvo.CQRS.Routing;

namespace NEvo.CQRS.Transporting;

public interface ITransportChannelFactory
{
    ITransportChannel CreateChannel(IServiceProvider serviceProvider, TransportChannelDescription channelDescription);
}

public interface ITransportChannelFactory<out TTransportChannel, in TChannelDescription, in TChannelTopologyDescription> where TTransportChannel : ITransportChannel where TChannelDescription : TransportChannelDescription where TChannelTopologyDescription : IChannelTopologyDescription
{
    TTransportChannel CreateChannel(IServiceProvider serviceProvider, TChannelDescription channelDescription, TChannelTopologyDescription topologyDescription);
}
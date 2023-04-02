using Microsoft.Extensions.DependencyInjection;
using NEvo.Azure.Publishing;
using NEvo.CQRS.Routing;
using NEvo.CQRS.Transporting;
using static NEvo.CQRS.Transporting.TransportChannelDescription;

namespace NEvo.Azure.Transporting;

public class AzureServiceBusTransportChannelFactory : ITransportChannelFactory<AzureServiceBusTransportChannel, MessagePublisherTransportChannelDescription, TopicTopologyDescription>
{
    public AzureServiceBusTransportChannel CreateChannel(IServiceProvider serviceProvider, MessagePublisherTransportChannelDescription channelDescription, TopicTopologyDescription topicTopologyDescription)
        => new AzureServiceBusTransportChannel(serviceProvider.GetRequiredService<IAzureServiceBusMessagePublisher>(), topicTopologyDescription.TopicName);
}

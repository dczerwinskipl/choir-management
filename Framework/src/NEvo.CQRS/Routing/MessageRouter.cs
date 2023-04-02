using NEvo.Core;
using NEvo.CQRS.Transporting;
using NEvo.CQRS.Messaging;

namespace NEvo.CQRS.Routing;

public class MessageRouter : IMessageRouter
{
    private readonly IRoutingPolicyFactory _routingPolicyFactory;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITransportChannelFactory _transportChannelFactory;

    public MessageRouter(ITransportChannelFactory transportChannelFactory, IRoutingPolicyFactory routingPolicyFactory, IServiceProvider serviceProvider)
    {
        _transportChannelFactory = Check.Null(transportChannelFactory);
        _routingPolicyFactory = Check.Null(routingPolicyFactory);
        _serviceProvider = Check.Null(serviceProvider);
    }

    public ITransportChannel ForMessage<TMessage>(TMessage message) where TMessage : IMessage
    {
        var routingPolicy = _routingPolicyFactory.CreatePolicyFor(message);
        var channelDescription = routingPolicy.GetChannelDescription();
        return _transportChannelFactory.CreateChannel(_serviceProvider, channelDescription);
    }
}
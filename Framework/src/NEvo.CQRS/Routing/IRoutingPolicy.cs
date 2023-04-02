using NEvo.CQRS.Transporting;
using NEvo.CQRS.Messaging;

namespace NEvo.CQRS.Routing;

public interface IRoutingPolicy
{
    TransportChannelDescription GetChannelDescription();
}

public interface IRoutingPolicyFactory
{
    IRoutingPolicy CreatePolicyFor<TMessage>(TMessage message) where TMessage : IMessage;
}
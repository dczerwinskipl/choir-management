using NEvo.CQRS.Messaging;
using NEvo.CQRS.Transporting;

namespace NEvo.CQRS.Routing;

public interface IRoutingPolicy
{
    TransportChannelDescription GetChannelDescription();
}

public interface IRoutingPolicyFactory
{
    IRoutingPolicy CreatePolicyFor<TMessage>(TMessage message) where TMessage : IMessage;
}
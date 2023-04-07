using NEvo.CQRS.Messaging;
using NEvo.CQRS.Transporting;

namespace NEvo.CQRS.Routing;

public interface IMessageRouter
{
    ITransportChannel ForMessage<TMessage>(TMessage message) where TMessage : IMessage;
}
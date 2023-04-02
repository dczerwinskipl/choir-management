using NEvo.CQRS.Transporting;
using NEvo.CQRS.Messaging;

namespace NEvo.CQRS.Routing;

public interface IMessageRouter
{
    ITransportChannel ForMessage<TMessage>(TMessage message) where TMessage : IMessage;
}
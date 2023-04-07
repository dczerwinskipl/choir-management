using NEvo.CQRS.Messaging;

namespace NEvo.CQRS.Processing.Registering;

public interface IMessageHandlerRegistry
{
    void Register<THandler>();
    IMessageHandlerAdapter GetHandler<TMessage, TResult>(TMessage message) where TMessage : IMessage<TResult>;
    IEnumerable<IMessageHandlerWrapper<TResult>> GetHandlers<TMessage, TResult>(TMessage message) where TMessage : IMessage<TResult>;
}

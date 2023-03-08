using NEvo.Messaging;
using System.ComponentModel;

namespace NEvo.Processing.Registering;

public interface IMessageHandlerRegistry
{
    void Register<THandler>();
    IMessageHandlerWrapper GetHandler<TMessage, TResult>(TMessage message) where TMessage : IMessage<TResult>;
    IEnumerable<IMessageHandlerWrapper<TResult>> GetHandlers<TMessage, TResult>(TMessage message) where TMessage : IMessage<TResult>;
}

using NEvo.Messaging;

namespace NEvo.Processing.Registering;

public interface IMessageHandlerRegistry
{
    void Register<THandler>();
    IMessageHandlerWrapper GetHandler(IMessage message);
    IEnumerable<IMessageHandlerWrapper> GetHandlers(IMessage message);
    IMessageHandlerWrapper<TResult> GetHandler<TResult>(IMessage<TResult> message);
    IEnumerable<IMessageHandlerWrapper<TResult>> GetHandlers<TResult>(IMessage<TResult> message);
}

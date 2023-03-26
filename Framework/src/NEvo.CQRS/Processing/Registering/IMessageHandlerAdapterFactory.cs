namespace NEvo.Processing.Registering;

public interface IMessageHandlerAdapterFactory
{
    IMessageHandlerAdapter Create(MessageHandlerDescription messageHandlerDescription, IServiceProvider provider);
    IEnumerable<MessageHandlerDescription> GetMessageHandlerDescriptions(Type handlerType, Type handlerInterface);
}

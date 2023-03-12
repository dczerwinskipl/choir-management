namespace NEvo.Processing.Registering;

public interface IMessageHandlerWrapperFactory
{
    IMessageHandlerWrapper Create(MessageHandlerDescription messageHandlerDescription, IServiceProvider provider);
}

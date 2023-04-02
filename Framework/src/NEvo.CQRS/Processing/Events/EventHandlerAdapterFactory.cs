using NEvo.Core;
using NEvo.CQRS.Processing.Registering;

namespace NEvo.CQRS.Processing.Events;

public class EventHandlerAdapterFactory : IMessageHandlerAdapterFactory
{
    public static MessageHandlerOptions MessageHandlerOptions = new MessageHandlerOptions(typeof(IEventHandler<>), new EventHandlerAdapterFactory());

    public IMessageHandlerAdapter Create(MessageHandlerDescription messageHandlerDescription, IServiceProvider provider)
    {
        var type = typeof(EventHandlerAdapter<,>).MakeGenericType(messageHandlerDescription.HandlerType, messageHandlerDescription.MessageType);
        var wrapper = Activator.CreateInstance(type, new object[] { messageHandlerDescription, provider });
        return Check.Null(wrapper as IMessageHandlerAdapter);
    }

    public IEnumerable<MessageHandlerDescription> GetMessageHandlerDescriptions(Type handlerType, Type handlerInterface)
    {
        yield return new MessageHandlerDescription(
                    handlerType,
                    handlerInterface.GetGenericArguments()[0],
                    handlerInterface,
                    handlerType.GetInterfaceMap(handlerInterface).TargetMethods[0]);
    }
}

using NEvo.Core;
using NEvo.Processing.Registering;

namespace NEvo.Processing.Commands;

public class CommandHandlerAdapterFactory : IMessageHandlerAdapterFactory
{
    public static MessageHandlerOptions MessageHandlerOptions = new MessageHandlerOptions(typeof(ICommandHandler<>), new CommandHandlerAdapterFactory());

    public IMessageHandlerAdapter Create(MessageHandlerDescription messageHandlerDescription, IServiceProvider provider)
    {
        var type = typeof(CommandHandlerAdapter<,>).MakeGenericType(messageHandlerDescription.HandlerType, messageHandlerDescription.MessageType);
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

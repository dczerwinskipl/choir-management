using NEvo.Core;
using NEvo.Processing.Commands;
using NEvo.Processing.Registering;

namespace NEvo.Processing.Commands;

public class CommandHandlerWrapperFactory : IMessageHandlerWrapperFactory
{
    public static MessageHandlerOptions MessageHandlerOptions = new MessageHandlerOptions(typeof(ICommandHandler<>), new CommandHandlerWrapperFactory());

    /// <summary>
    /// Maybe it should just be a static method?
    /// </summary>
    /// <param name="messageHandlerDescription"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public IMessageHandlerWrapper Create(MessageHandlerDescription messageHandlerDescription, IServiceProvider provider)
    {
        var type = typeof(CommandHandlerWrapper<,>).MakeGenericType(messageHandlerDescription.HandlerType, messageHandlerDescription.MessageType);
        var wrapper = Activator.CreateInstance(type, new object[] { messageHandlerDescription, provider });
        return Check.Null(wrapper as IMessageHandlerWrapper);
    }
}

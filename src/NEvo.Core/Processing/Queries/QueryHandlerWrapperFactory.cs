using NEvo.Core;
using NEvo.Processing.Registering;

namespace NEvo.Processing.Queries;

public class QueryHandlerWrapperFactory : IMessageHandlerWrapperFactory
{
    public static MessageHandlerOptions MessageHandlerOptions = new MessageHandlerOptions(typeof(IQueryHandler<,>), new QueryHandlerWrapperFactory());

    /// <summary>
    /// Maybe it should just be a static method?
    /// </summary>
    /// <param name="messageHandlerDescription"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public IMessageHandlerWrapper Create(MessageHandlerDescription messageHandlerDescription, IServiceProvider provider)
    {
        var type = typeof(QueryHandlerWrapper<,,>).MakeGenericType(messageHandlerDescription.HandlerType, messageHandlerDescription.MessageType, messageHandlerDescription.InterfaceType.GetGenericArguments()[1]);
        var wrapper = Activator.CreateInstance(type, new object[] { messageHandlerDescription, provider });
        return Check.Null(wrapper as IMessageHandlerWrapper);
    }
}

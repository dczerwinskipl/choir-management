using NEvo.Core;
using NEvo.CQRS.Processing.Registering;

namespace NEvo.CQRS.Processing.Queries;

public class QueryHandlerAdapterFactory : IMessageHandlerAdapterFactory
{
    public static MessageHandlerOptions MessageHandlerOptions = new MessageHandlerOptions(typeof(IQueryHandler<,>), new QueryHandlerAdapterFactory());

    public IMessageHandlerAdapter Create(MessageHandlerDescription messageHandlerDescription, IServiceProvider provider)
    {
        var type = typeof(QueryHandlerAdapter<,,>).MakeGenericType(messageHandlerDescription.HandlerType, messageHandlerDescription.MessageType, messageHandlerDescription.InterfaceType.GetGenericArguments()[1]);
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

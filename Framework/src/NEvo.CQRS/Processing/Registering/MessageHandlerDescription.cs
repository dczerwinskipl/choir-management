using System.Reflection;

namespace NEvo.CQRS.Processing.Registering;

public record MessageHandlerDescription(Type HandlerType, Type MessageType /* todo: Change to MessageType */, Type InterfaceType, MethodInfo Method)
{
    /*public MessageType MessageType =>
        typeof(Command).IsAssignableFrom(MessageClass) ? MessageType.Command :
        typeof(Event).IsAssignableFrom(MessageClass) ? MessageType.Event :
                                                       MessageType.Query;*/
}

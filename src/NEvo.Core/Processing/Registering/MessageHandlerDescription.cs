using NEvo.Messaging;
using NEvo.Messaging.Commands;
using NEvo.Messaging.Events;
using System.ComponentModel.Design;
using System.Reflection;

namespace NEvo.Processing.Registering;

public record MessageHandlerDescription(Type HandlerType, Type MessageClass /* todo: Change to MessageType */, Type InterfaceType,MethodInfo Method)
{
    public MessageType MessageType =>
        typeof(Command).IsAssignableFrom(MessageClass) ? MessageType.Command :
        typeof(Event).IsAssignableFrom(MessageClass) ? MessageType.Event :
                                                       MessageType.Query;
}

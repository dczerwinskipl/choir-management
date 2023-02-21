﻿using NEvo.Core;
using NEvo.Processing.Registering;

namespace NEvo.Processing.Events;

public class EventHandlerWrapperFactory : IMessageHandlerWrapperFactory
{
    public static MessageHandlerOptions MessageHandlerOptions = new MessageHandlerOptions(typeof(IEventHandler<>), new EventHandlerWrapperFactory());

    /// <summary>
    /// Maybe it should just be a static method?
    /// </summary>
    /// <param name="messageHandlerDescription"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public IMessageHandlerWrapper Create(MessageHandlerDescription messageHandlerDescription, IServiceProvider provider)
    {
        var type = typeof(EventHandlerWrapper<,>).MakeGenericType(messageHandlerDescription.HandlerType, messageHandlerDescription.MessageType);
        var wrapper = Activator.CreateInstance(type, new object[] { messageHandlerDescription, provider });
        return Check.Null(wrapper as IMessageHandlerWrapper);
    }
}

﻿using NEvo.CQRS.Messaging;
using System.Collections.Concurrent;

namespace NEvo.CQRS.Channeling;

public class AttributeChannelProvider
{
    private ConcurrentDictionary<Type, string?> _channels = new ConcurrentDictionary<Type, string?>();
    public string? ChannelFor<TMessage>(MessageEnvelope<TMessage> messageEnvelope) where TMessage : IMessage 
        => _channels.GetOrAdd(typeof(TMessage), ChannelFor);

    private string? ChannelFor(Type messageType)
    {
        var attributes = messageType.GetCustomAttributes(typeof(ChannelAttribute), false);
        if(attributes.Any() && attributes.Single() is ChannelAttribute channelAttribute)
        {
            return channelAttribute.Name;
        }
        return null;
    }
}
using NEvo.Core;
using NEvo.Messaging;
using System.ComponentModel;

namespace NEvo.Processing.Registering;

public class MessageHandlerRegistry : IMessageHandlerRegistry
{
    private readonly ConcurrentMultivalueDictionary<Type, IMessageHandlerWrapper> _handlers = new();

    private readonly IServiceProvider _serviceProvider;
    private readonly IDictionary<Type, MessageHandlerOptions> _messageHandlerOptions;
    private readonly IDictionary<MessageType, MessageHandlingOptions> _messagesOptions;

    public MessageHandlerRegistry(IServiceProvider serviceProvider, IEnumerable<MessageHandlerOptions> handlerOptions, IDictionary<MessageType, MessageHandlingOptions> messagesOptions /* add as IOptions with validation */) 
    {
        _serviceProvider = Check.Null(serviceProvider);
        _messageHandlerOptions = Check.Null(handlerOptions).ToDictionary(k => k.HandlerInterface, k => k);
        _messagesOptions = Check.Null(messagesOptions);
    }

    public MessageHandlerRegistry(IServiceProvider serviceProvider, params MessageHandlerOptions[] handlerOptions /* TODO: przywrócić listę? */) : this(serviceProvider, handlerOptions, MessageHandlingOptions.DefaultMessageHandlingOptions)
    {
    }

    public MessageHandlerRegistry(IServiceProvider serviceProvider) : this(serviceProvider, Enumerable.Empty<MessageHandlerOptions>(), MessageHandlingOptions.DefaultMessageHandlingOptions)
    { 
    }

    public IEnumerable<IMessageHandlerWrapper<TResult>> GetHandlers<TMessage, TResult>(TMessage message) where TMessage: IMessage<TResult>
    {
        var messageOptions = _messagesOptions[TMessage.MessageType];
        var allHandlers = _handlers.TryGetValue(message.GetType(), out var handlers) ? 
            handlers.Select(h => h.ToGeneric<TResult>()) : 
            Enumerable.Empty<IMessageHandlerWrapper<TResult>>();
        
        if (!messageOptions.AllowMultipleHandlers && allHandlers.Count() > 1)
            throw new MultipleHandlersFoundException(message.GetType());

        if (messageOptions.RequireHandler && !allHandlers.Any())
            throw new HandlerNotFoundException(message.GetType());

        return allHandlers;
    }

    public IMessageHandlerWrapper GetHandler<TMessage, TResult>(TMessage message) where TMessage : IMessage<TResult> => GetHandlers<TMessage, TResult>(message).Single();

    public void Register<THandler>()
    {
        var handlerType = typeof(THandler);
        var messageTypes = GetHandlersDescriptions(handlerType);

        foreach (var handlerDescription in messageTypes)
        {
            var messageType = GetMessageType(handlerDescription.MessageType);
            var messageOptions = _messagesOptions[messageType];
            var messageHandlerOptions = _messageHandlerOptions[handlerDescription.InterfaceType.GetGenericTypeDefinition()];

            if (!messageOptions.AllowMultipleHandlers && _handlers.ContainsKey(handlerDescription.MessageType))
                throw new HandlerAlreadyRegisterdException(handlerDescription.MessageType);

            _handlers.Add(handlerDescription.MessageType, messageHandlerOptions.MessageHandlerWrapperFactory.Create(handlerDescription, _serviceProvider));
        }
    }

    private MessageType GetMessageType(Type messageClass) =>
        (MessageType)messageClass
            .GetProperty(nameof(IMessage.MessageType), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.FlattenHierarchy)
            .GetValue(null);

    protected IEnumerable<MessageHandlerDescription> GetHandlersDescriptions(Type handlerType) =>
        handlerType
            .GetInterfaces()
            .Where(handlerInterface => handlerInterface.IsGenericType && _messageHandlerOptions.Keys.Contains(handlerInterface.GetGenericTypeDefinition()))
            .Select(handlerInterface =>
                 new MessageHandlerDescription(
                    handlerType,
                    handlerInterface.GetGenericArguments()[0],
                    handlerInterface,
                    handlerType.GetInterfaceMap(handlerInterface).TargetMethods[0])
            );
}
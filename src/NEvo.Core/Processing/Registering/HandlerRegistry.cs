using NEvo.Core;
using NEvo.Messaging;

namespace NEvo.Processing.Registering;

public class HandlerRegistry : IMessageHandlerRegistry
{
    private readonly ConcurrentMultivalueDictionary<Type, IMessageHandlerWrapper> _handlers = new();

    private readonly IServiceProvider _serviceProvider;
    private readonly IDictionary<Type, MessageHandlerOptions> _messageHandlerOptions;
    private readonly IDictionary<MessageType, MessageHandlingOptions> _messagesOptions;

    public HandlerRegistry(IServiceProvider serviceProvider, IEnumerable<MessageHandlerOptions> handlerOptions, IDictionary<MessageType, MessageHandlingOptions> messagesOptions /* add as IOptions with validation */) 
    {
        _serviceProvider = Check.Null(serviceProvider);
        _messageHandlerOptions = Check.Null(handlerOptions).ToDictionary(k => k.HandlerInterface, k => k);
        _messagesOptions = Check.Null(messagesOptions);
    }

    public HandlerRegistry(IServiceProvider serviceProvider, params MessageHandlerOptions[] handlerOptions /* TODO: przywrócić listę? */) : this(serviceProvider, handlerOptions, MessageHandlingOptions.DefaultMessageHandlingOptions)
    {
    }

    public HandlerRegistry(IServiceProvider serviceProvider) : this(serviceProvider, Enumerable.Empty<MessageHandlerOptions>(), MessageHandlingOptions.DefaultMessageHandlingOptions)
    { 
    }

    public IEnumerable<IMessageHandlerWrapper> GetHandlers(IMessage message)
    {
        var messageOptions = _messagesOptions[message.MessageType];
        var allHandlers = _handlers.TryGetValue(message.GetType(), out var handlers) ? handlers : Enumerable.Empty<IMessageHandlerWrapper>();
        
        if (!messageOptions.AllowMultipleHandlers && allHandlers.Count() > 1)
            throw new MultipleHandlersFoundException(message.GetType());

        if (messageOptions.RequireHandler && !allHandlers.Any())
            throw new HandlerNotFoundException(message.GetType());

        return allHandlers;
    }

    public IEnumerable<IMessageHandlerWrapper<TResult>> GetHandlers<TResult>(IMessage<TResult> message)
    {
        throw new NotImplementedException();
    }

    public IMessageHandlerWrapper GetHandler(IMessage message) => GetHandlers(message).Single();

    public IMessageHandlerWrapper<TResult> GetHandler<TResult>(IMessage<TResult> message) => GetHandlers(message).Single();

    public void Register<THandler>()
    {
        var handlerType = typeof(THandler);
        var messageTypes = GetHandlersDescriptions(handlerType);

        foreach (var handlerDescription in messageTypes)
        {
            var messageOptions = _messagesOptions[handlerDescription.MessageType];
            var messageHandlerOptions = _messageHandlerOptions[handlerDescription.InterfaceType.GetGenericTypeDefinition()];

            if (!messageOptions.AllowMultipleHandlers && _handlers.ContainsKey(handlerDescription.MessageClass))
                throw new HandlerAlreadyRegisterdException(handlerDescription.MessageClass);

            _handlers.Add(handlerDescription.MessageClass, messageHandlerOptions.MessageHandlerWrapperFactory.Create(handlerDescription, _serviceProvider));
        }
    }

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
namespace NEvo.CQRS.Processing.Registering;

public record MessageHandlerOptions(Type HandlerInterface, IMessageHandlerAdapterFactory MessageHandlerAdapterFactory);

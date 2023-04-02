namespace NEvo.CQRS.Processing.Registering;

public class HandlerNotFoundException : HandlerRegistryException
{
    public HandlerNotFoundException(Type messageType) : base(messageType) { }
}

namespace NEvo.Processing.Registering;

public class HandlerNotFoundException : HandlerRegistryException
{
    public HandlerNotFoundException(Type messageType) : base(messageType) { }
}

namespace NEvo.Processing.Registering;

public class HandlerAlreadyRegisterdException : HandlerRegistryException
{
    public HandlerAlreadyRegisterdException(Type messageType) : base(messageType) { }
}

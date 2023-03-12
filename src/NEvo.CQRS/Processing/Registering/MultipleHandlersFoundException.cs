namespace NEvo.Processing.Registering;

public class MultipleHandlersFoundException : HandlerRegistryException
{
    public MultipleHandlersFoundException(Type messageType) : base(messageType) { }
}

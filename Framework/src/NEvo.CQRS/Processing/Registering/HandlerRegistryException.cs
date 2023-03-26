namespace NEvo.Processing.Registering;

public abstract class HandlerRegistryException : Exception
{
    public Type MessageType { get; }
    public HandlerRegistryException(Type messageType)
    {
        MessageType = messageType;
    }
}

namespace NEvo.Messaging;

public abstract class Message : IMessage
{
    public abstract MessageType MessageType { get; }
    public DateTime CreatedAt { get; protected set; } = DateTime.Now;
}
namespace NEvo.Messaging;

public enum MessageType
{
    Command = 1,
    Event = 2,
    Query = 3
}

public interface IMessage
{
    public static abstract MessageType MessageType { get; }
}

public interface IMessage<out TResposne> : IMessage
{
    DateTime CreatedAt { get; }
}
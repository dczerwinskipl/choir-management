namespace NEvo.Messaging;

public enum MessageType
{
    Command = 1,
    Event = 2,
    Query = 3
}

public interface IMessage
{
    MessageType MessageType { get;}
    DateTime CreatedAt { get; }
}

public interface IMessage<out TResposne> : IMessage
{

}
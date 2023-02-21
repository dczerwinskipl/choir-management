namespace NEvo.Messaging.Queries;

public abstract class Query<TResult> : IMessage<TResult>
{
    public static MessageType MessageType => MessageType.Query;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

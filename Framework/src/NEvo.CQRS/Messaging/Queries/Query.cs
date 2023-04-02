namespace NEvo.CQRS.Messaging.Queries;

public abstract record Query<TResult> : IMessage<TResult>
{
    public static MessageType MessageType => MessageType.Query;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

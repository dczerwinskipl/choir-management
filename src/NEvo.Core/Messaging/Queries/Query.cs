namespace NEvo.Messaging.Queries;

public abstract class Query<TResult> : Message, IMessage<TResult>
{
    public override sealed MessageType MessageType => MessageType.Query;
}

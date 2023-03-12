using NEvo.Core;
using NEvo.ValueObjects;

namespace NEvo.Messaging.Events;

public abstract record Event : IMessage<Unit>
{
    public static MessageType MessageType => MessageType.Event;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ObjectId? Source { get; set; }

    protected Event(ObjectId source)
    {
        Source = source; // Check.Null(source); deserialization problem?
    }
}

using NEvo.Core;
using NEvo.ValueObjects;

namespace NEvo.Messaging.Events;

public abstract class Event : IMessage<Unit>
{
    public static MessageType MessageType => MessageType.Event;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public SourceId? Source { get; }

    protected Event() { }
    protected Event(SourceId source)
    {
        Source = Check.Null(source);
    }
}

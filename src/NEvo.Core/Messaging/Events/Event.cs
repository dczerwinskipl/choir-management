using NEvo.Core;
using NEvo.Core.ValueObjects;

namespace NEvo.Messaging.Events;

public abstract class Event : Message 
{
    public override sealed MessageType MessageType => MessageType.Event;

    public SourceId? Source { get; }

    protected Event() { }
    protected Event(SourceId source)
    {
        Source = Check.Null(source);
    }
}

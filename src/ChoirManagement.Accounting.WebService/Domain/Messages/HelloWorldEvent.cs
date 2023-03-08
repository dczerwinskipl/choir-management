using NEvo.Messaging.Events;
using NEvo.ValueObjects;

namespace ChoirManagement.Accounting.Messages;

public record HelloWorldEvent(string Message, SourceId Source) : Event(Source);

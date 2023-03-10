using NEvo.Messaging;
using NEvo.Messaging.Events;

namespace NEvo.Core.Publishing.Outboxing;

public interface IEventOutbox
{
    Task SaveAsync<TEvent>(MessageEnvelope<TEvent> @event) where TEvent : Event;
}

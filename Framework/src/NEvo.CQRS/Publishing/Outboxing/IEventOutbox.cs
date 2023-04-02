using NEvo.CQRS.Messaging;
using NEvo.CQRS.Messaging.Events;

namespace NEvo.Publishing.Outboxing;

public interface IEventOutbox
{
    Task SaveAsync<TEvent>(MessageEnvelope<TEvent> @event) where TEvent : Event;
}

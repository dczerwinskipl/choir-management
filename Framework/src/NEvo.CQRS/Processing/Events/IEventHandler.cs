using NEvo.CQRS.Messaging.Events;

namespace NEvo.CQRS.Processing.Events;

public interface IEventHandler<in TEvent> where TEvent : Event
{
    Task HandleAsync(TEvent @event);
}
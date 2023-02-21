using NEvo.Messaging.Events;

namespace NEvo.Processing.Events;

public interface IEventHandler<in TEvent> where TEvent : Event
{
    Task HandleAsync(TEvent @event);
}
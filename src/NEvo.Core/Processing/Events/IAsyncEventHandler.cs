using NEvo.Messaging.Events;

namespace NEvo.Processing.Events;

public interface IAsyncEventHandler<TEvent> where TEvent : Event
{
    Task HandleAsync(TEvent @event);
}

using NEvo.Messaging.Events;

namespace NEvo.Publishing;

public interface IEventBus
{
    Task PublishAsync(Event @event);
}

using NEvo.Messaging.Queries;

namespace NEvo.Messaging.Events;

public interface IEventPublisher
{
    void Publish(Event @event) => PublishAsync(@event).ConfigureAwait(false).GetAwaiter().GetResult();
    Task PublishAsync(Event @event);
}

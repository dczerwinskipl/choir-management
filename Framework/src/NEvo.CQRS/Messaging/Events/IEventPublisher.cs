namespace NEvo.CQRS.Messaging.Events;

public interface IEventPublisher
{
    void Publish<TEvent>(TEvent @event) where TEvent : Event => PublishAsync(@event).ConfigureAwait(false).GetAwaiter().GetResult();
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : Event;
}
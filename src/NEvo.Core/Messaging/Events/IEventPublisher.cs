using NEvo.Core;
using NEvo.Messaging.Queries;

namespace NEvo.Messaging.Events;

public interface IEventPublisher
{
    void Publish(Event @event) => PublishAsync(@event).ConfigureAwait(false).GetAwaiter().GetResult().OnFailure(exc => throw exc);
    Task<Either<Exception, Unit>> PublishAsync(Event @event);
}

using NEvo.Messaging.Commands;
using NEvo.Messaging.Events;
using NEvo.Messaging.Queries;

namespace NEvo.Messaging;

public interface IMessageBus : ICommandDispatcher, IEventPublisher, IQueryDispatcher
{
}

using NEvo.CQRS.Messaging.Commands;
using NEvo.CQRS.Messaging.Events;
using NEvo.CQRS.Messaging.Queries;

namespace NEvo.CQRS.Messaging;

/// <summary>
/// TODO: change name of this interface (or from publishing namespace), its not the same as external message bus system 
/// </summary>
public interface IMessageBus : ICommandDispatcher, IEventPublisher, IQueryDispatcher
{
}

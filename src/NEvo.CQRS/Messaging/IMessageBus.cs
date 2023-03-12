using NEvo.Messaging.Commands;
using NEvo.Messaging.Events;
using NEvo.Messaging.Queries;

namespace NEvo.Messaging;

/// <summary>
/// TODO: change name of this interface (or from publishing namespace), its not the same as external message bus system 
/// </summary>
public interface IMessageBus : ICommandDispatcher, IEventPublisher, IQueryDispatcher
{
}

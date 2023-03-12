using NEvo.Core;
using NEvo.Messaging.Commands;

namespace NEvo.Processing.Commands;

public interface ICommandHandler<in TCommand> where TCommand : Command
{
    Task<Try<Unit>> HandleAsync(TCommand command);
}
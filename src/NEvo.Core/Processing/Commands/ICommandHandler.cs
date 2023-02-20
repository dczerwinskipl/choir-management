using NEvo.Messaging.Commands;

namespace NEvo.Processing.Commands;

public interface ICommandHandler<TCommand> where TCommand : Command
{
    Task HandleAsync(TCommand command);
}
using NEvo.Core;
using NEvo.Messaging.Commands;

namespace NEvo.Processing.Commands;

public interface ICommandHandler<TCommand> where TCommand : Command
{
    Task<Either<Exception, Unit>> HandleAsync(TCommand command);
}
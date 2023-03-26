using NEvo.Core;
using NEvo.Monads;
using NEvo.Messaging.Commands;

namespace NEvo.Processing.Commands;

public interface ICommandHandler<in TCommand> where TCommand : Command
{
    Task<Either<Exception, Unit>> HandleAsync(TCommand command);
}
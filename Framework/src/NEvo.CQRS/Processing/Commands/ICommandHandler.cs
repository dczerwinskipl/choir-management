using NEvo.Core;
using NEvo.Monads;
using NEvo.CQRS.Messaging.Commands;

namespace NEvo.CQRS.Processing.Commands;

public interface ICommandHandler<in TCommand> where TCommand : Command
{
    Task<Either<Exception, Unit>> HandleAsync(TCommand command);
}
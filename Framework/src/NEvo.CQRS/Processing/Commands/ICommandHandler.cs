using NEvo.Core;
using NEvo.CQRS.Messaging.Commands;
using NEvo.Monads;

namespace NEvo.CQRS.Processing.Commands;

public interface ICommandHandler<in TCommand> where TCommand : Command
{
    Task<Either<Exception, Unit>> HandleAsync(TCommand command);
}
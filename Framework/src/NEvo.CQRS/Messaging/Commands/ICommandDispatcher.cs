using NEvo.Core;
using NEvo.Monads;

namespace NEvo.CQRS.Messaging.Commands;

public interface ICommandDispatcher
{
    void Dispatch<TCommand>(TCommand command) where TCommand : Command => DispatchAsync(command).ConfigureAwait(false).GetAwaiter().GetResult().OnFailure(exc => throw exc);
    Task<Either<Exception, Unit>> DispatchAsync<TCommand>(TCommand command) where TCommand : Command;
}


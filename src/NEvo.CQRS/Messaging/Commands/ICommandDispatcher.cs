using NEvo.Core;
using NEvo.Monads;

namespace NEvo.Messaging.Commands;

public interface ICommandDispatcher
{
    void Dispatch(Command command) => DispatchAsync(command).ConfigureAwait(false).GetAwaiter().GetResult().OnFailure(exc => throw exc);
    Task<Either<Exception, Unit>> DispatchAsync(Command command);
}


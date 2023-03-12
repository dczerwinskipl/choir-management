using NEvo.Core;
using NEvo.Messaging.Commands;

namespace NEvo.Messaging.Commands;

public interface ICommandDispatcher
{
    void Dispatch(Command command) => DispatchAsync(command).ConfigureAwait(false).GetAwaiter().GetResult().OnFailure(exc => throw exc);
    Task<Try<Unit>> DispatchAsync(Command command);
}


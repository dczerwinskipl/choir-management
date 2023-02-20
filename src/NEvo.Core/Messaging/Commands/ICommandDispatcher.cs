using NEvo.Messaging.Commands;

namespace NEvo.Messaging.Commands;

public interface ICommandDispatcher
{
    void Dispatch(Command command) => DispatchAsync(command).ConfigureAwait(false).GetAwaiter().GetResult();
    Task DispatchAsync(Command command);
}


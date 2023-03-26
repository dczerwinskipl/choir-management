using NEvo.Monads;

namespace NEvo.Core.StateManaging;

public interface IState<TContext>
{
    string Name { get; }

    Task<Either<Exception, TContext>> OnEnterAsync(TContext context);
    Task<Either<Exception, TContext>> OnExitAsync(TContext context);
}

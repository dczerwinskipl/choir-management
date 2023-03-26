using NEvo.Monads;

namespace NEvo.Core.StateManaging;

public interface ITransition<TContext, TTransitionData> : ITransition<TContext>
{
    Task<Either<Refused, IState<TContext>>> ApplyTransitionAsync(TContext context, TTransitionData data);
}

using NEvo.Monads;

namespace NEvo.Core.StateManaging;

public interface IStateMachineDefinition<TContext>
{
    IState<TContext> GetInitState();
    Maybe<Unit> IsDefined(ITransition<TContext> transition, Maybe<IState<TContext>> currentState);
}

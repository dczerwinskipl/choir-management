using Microsoft.AspNetCore.Mvc.Filters;
using NEvo.Core;
using NEvo.Monads;

namespace NEvo.Core.StateManaging;

public class StateMachine<TContext>
{
    public TContext Context { get; private set; }

    private Maybe<IState<TContext>> _currentState;
    public IState<TContext> CurrentState { get { return _currentState.Value; } private set { _currentState = Maybe.Some(value); } }

    private readonly IStateMachineDefinition<TContext> _stateMachineDefinition;

    public StateMachine(Maybe<IState<TContext>> initState, TContext initContext, IStateMachineDefinition<TContext> stateMachineDefinition)
    {
        Context = initContext;
        _stateMachineDefinition = stateMachineDefinition;
        _currentState = initState;
    }

    public StateMachine(IState<TContext> initState, TContext initContext, IStateMachineDefinition<TContext> stateMachineDefinition)
        : this(Maybe.Some(initState), initContext, stateMachineDefinition)
    {
    }

    public StateMachine(TContext initContext, IStateMachineDefinition<TContext> stateMachineDefinition)
    : this(Maybe.None, initContext, stateMachineDefinition)
    {
    }

    public async Task<Either<Exception, Unit>> InitAsync() =>
        await _currentState.MatchAsync(
                none: async () => await ApplyState(_stateMachineDefinition.GetInitState()),
                some: (_) => Unit.Value                
            );

    public async Task<Either<Refused, Unit>> ApplyTransitionAsync<TData>(ITransition<TContext, TData> transition, TData data) => 
        await _stateMachineDefinition
                .IsDefined(transition, _currentState)
                .MatchAsync(
                    async some => await transition
                                       .ApplyTransitionAsync(Context, data)
                                       .ThenAsync(ApplyState, (exc) => new Refused.StateError(exc)),
                    () => new Refused.TransitionNotFound(transition.Name, _currentState.Match(some => some.Name, () => string.Empty), $"Transition not defined")
                );

    private async Task<Either<Exception, Unit>> ApplyState(IState<TContext> newState) =>
    await _currentState
                    .MatchAsync(
                        async previousState => await previousState.OnExitAsync(Context),
                        () => Context
                    )
                    .ThenAsync(newState.OnEnterAsync)
                    .Then((context) => {
                        Context = context;
                        CurrentState = newState;
                    });                     
}

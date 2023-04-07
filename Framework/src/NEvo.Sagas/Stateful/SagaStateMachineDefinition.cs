using NEvo.Core;
using NEvo.Core.StateManaging;
using NEvo.Monads;

namespace NEvo.Sagas.Stateful;

public class SagaStateMachineDefinition<TSaga, TState> : IStateMachineDefinition<TSaga> where TSaga : IStatefulSaga<TState>
{
    private readonly IState<TSaga> _initState;
    private readonly TSaga _emptySaga;
    private readonly IStateMapper<TState, IState<TSaga>> _stateMapper;
    private readonly Dictionary<string, IState<TSaga>> _states;
    private readonly Dictionary<IState<TSaga>, List<ISagaMessageTransition<TSaga>>> _transitions;

    public SagaStateMachineDefinition(IState<TSaga> initState, TSaga emptySaga, IStateMapper<TState, IState<TSaga>> stateMapper, Dictionary<string, IState<TSaga>> states, Dictionary<IState<TSaga>, List<ISagaMessageTransition<TSaga>>> transitions)
    {
        _initState = Check.Null(initState);
        _emptySaga = Check.Null(emptySaga);
        _stateMapper = Check.Null(stateMapper);
        _states = Check.Null(states);
        _transitions = Check.Null(transitions);
    }

    public IState<TSaga> GetInitState() => _initState;

    public Maybe<Unit> IsDefined(ITransition<TSaga> transition, Maybe<IState<TSaga>> currentState)
        => currentState
            .Bind(state => _transitions.MaybeGetValue(state))
            .Bind(transitions => transitions.MaybeFirst(t => t == transition))
            .Bind(success => Maybe.Some(Unit.Value));

    public async Task<Either<Exception, StateMachine<TSaga>>> NewSagaStateMachineAsync() =>
        await Try.OfAsync(async () =>
        {
            var stateMachine = new StateMachine<TSaga>(GetInitState(), _emptySaga, this);
            await stateMachine.InitAsync();
            SaveStateOnContext(stateMachine);
            return stateMachine;
        });

    public void SaveStateOnContext(StateMachine<TSaga> stateMachine)
        => stateMachine.Context.State = _stateMapper.MapFromState(stateMachine.CurrentState);

    public async Task<Either<Exception, StateMachine<TSaga>>> LoadSagaStateMachineAsync(TSaga fromContext) =>
        _stateMapper
            .MapToState(fromContext.State)
            .Match(some => some, () => new InvalidOperationException("Current state not found"))
            .Bind(state => new StateMachine<TSaga>(state, fromContext, this));

    public Maybe<ITransition<TSaga, TTransitionData>> GetMessageTransition<TTransitionData>(IState<TSaga> currentState) =>
        _transitions
            .MaybeGetValue(currentState)
            .Bind(transitions => transitions.MaybeFirst(t => t.MessageType == typeof(TTransitionData)))
            .Cast<ITransition<TSaga, TTransitionData>>();

    public TState MapFromState(IState<TSaga> currentState) => _stateMapper.MapFromState(currentState);
}

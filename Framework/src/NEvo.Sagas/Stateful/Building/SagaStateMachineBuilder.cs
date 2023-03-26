using NEvo.Core;
using NEvo.Core.StateManaging;
using NEvo.Monads;

namespace NEvo.Sagas.Stateful.Building;

public class SagaStateMachineBuilder<TSaga, TState> where TSaga : IStatefulSaga<TState>
{
    protected TSaga? _emptySaga;
    protected IState<TSaga>? _initState;
    protected IStateMapper<TState, IState<TSaga>>? _stateMapper;
    protected Dictionary<string, IInternalSagaStateBuilder<TSaga>> _statesBuilder = new Dictionary<string, IInternalSagaStateBuilder<TSaga>>();

    public SagaStateMachineBuilder()
    {
    }

    public SagaStateMachineBuilder<TSaga, TState> UseInitState(IState<TSaga> state) { _initState = state; return this; }
    public SagaStateMachineBuilder<TSaga, TState> UseInitState<T>() where T : IState<TSaga>, new() => UseInitState(new T());
    public SagaStateMachineBuilder<TSaga, TState> UseEmptySaga(TSaga saga) { _emptySaga = saga; return this; }
    public SagaStateMachineBuilder<TSaga, TState> UseEmptySaga<T>() where T : TSaga, new() => UseEmptySaga(new T());
    public SagaStateMachineBuilder<TSaga, TState> UseStateMapper(IStateMapper<TState, IState<TSaga>> stateMapper) { _stateMapper = stateMapper; return this; }
    public SagaStateMachineBuilder<TSaga, TState> UseStateMapper<T>() where T : IStateMapper<TState, IState<TSaga>>, new() => UseStateMapper(new T());


    public SagaStateMachineBuilder<TSaga, TState> AddInitState(string name, params Action<SagaStateBuilder<TSaga, TState>>[] configureState)
        => AddState(name, configureState.Append(builder => builder.InitState()).ToArray());
    public SagaStateMachineBuilder<TSaga, TState> AddState(string name, params Action<SagaStateBuilder<TSaga, TState>>[] configureState)
    {
        var builder = new SagaStateBuilder<TSaga, TState>(name);
        _statesBuilder.Add(name, builder);
        ConfigureStateInternal(builder, configureState);
        return this;
    }

    public SagaStateMachineBuilder<TSaga, TState> ConfigureState(string name, params Action<SagaStateBuilder<TSaga, TState>>[] configureState)
    {
        if (!_statesBuilder.TryGetValue(name, out var builder))
            throw new Exception($"State '{name}' not found");

        ConfigureStateInternal(builder as SagaStateBuilder<TSaga, TState>, configureState);
        return this;
    }

    private void ConfigureStateInternal(SagaStateBuilder<TSaga, TState> builder, params Action<SagaStateBuilder<TSaga, TState>>[] configureState)
    {
        foreach (var configure in configureState)
            configure(builder);
    }

    public virtual SagaStateMachineDefinition<TSaga, TState> Build()
    {
        BuildStatesAndTransitions(out var states, out var transitions);

        // defaults if not set already?
        if (_stateMapper is null && typeof(TState) == typeof(string))
        {
            _stateMapper = (IStateMapper<TState, IState<TSaga>>)new StringStateMapper<TSaga>(states.Values);
        }

        return BuildSagaDefinition(states, transitions);
    }

    protected virtual SagaStateMachineDefinition<TSaga, TState> BuildSagaDefinition(Dictionary<string, IState<TSaga>> states, Dictionary<IState<TSaga>, List<ISagaMessageTransition<TSaga>>> transitions) => new SagaStateMachineDefinition<TSaga, TState>(
                    Check.Null(_initState),
                    Check.Null(_emptySaga),
                    Check.Null(_stateMapper),
                    states,
                    transitions
                );

    protected virtual void BuildStatesAndTransitions(out Dictionary<string, IState<TSaga>> states, out Dictionary<IState<TSaga>, List<ISagaMessageTransition<TSaga>>> transitions)
    {
        var statesaAndTransitions = _statesBuilder.Values.Select(b => { var state = b.Build(); if (b.IsInitState) _initState = state.State; return state; });
        states = statesaAndTransitions.Select(s => s.State).ToDictionary(s => s.Name);
        var internalStates = states;
        transitions = statesaAndTransitions.ToDictionary(
                                                                st => st.State,
                                                                st => st.TransitionBuilders.Select(tb => tb.Build(internalStates)).UnPack().Cast<ISagaMessageTransition<TSaga>>().ToList()
                                                            );
    }
}

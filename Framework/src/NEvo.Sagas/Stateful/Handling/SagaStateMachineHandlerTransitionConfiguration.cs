using NEvo.Sagas.Stateful.Building;

namespace NEvo.Sagas.Stateful.Handling;

public abstract class SagaStateMachineHandlerTransitionConfiguration<TSaga> where TSaga : ISaga
{
    private List<Action<ISagaTransitionBuilder<TSaga>>> _actions = new List<Action<ISagaTransitionBuilder<TSaga>>>();

    public SagaStateMachineHandlerTransitionConfiguration<TSaga> TransitionTo(string stateName)
    {
        _actions.Add(builder => builder.TransitionTo(stateName));
        return this;
    }

    protected abstract ISagaTransitionBuilder<TSaga> AddTransition<TState>(SagaStateBuilder<TSaga, TState> sagaStateBuilder);
    public void Apply<TState>(SagaStateBuilder<TSaga, TState> sagaStateBuilder) => _actions.ForEach(action => action(AddTransition(sagaStateBuilder)));
}

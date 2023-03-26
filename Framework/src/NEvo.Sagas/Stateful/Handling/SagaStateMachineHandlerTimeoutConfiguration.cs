using NEvo.Sagas.Stateful.Building;

namespace NEvo.Sagas.Stateful.Handling;

public class SagaStateMachineHandlerTimeoutConfiguration<TSaga> : SagaStateMachineHandlerTransitionConfiguration<TSaga> where TSaga : ISaga
{
    private TimeSpan _timeout;

    public SagaStateMachineHandlerTimeoutConfiguration(TimeSpan timeout)
    {
        _timeout = timeout;
    }

    protected override ISagaTransitionBuilder<TSaga> AddTransition<TState>(SagaStateBuilder<TSaga, TState> sagaStateBuilder)
    {
        throw new NotImplementedException();
    }
}
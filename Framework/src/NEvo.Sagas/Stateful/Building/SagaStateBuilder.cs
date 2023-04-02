using NEvo.Core.StateManaging;
using NEvo.CQRS.Messaging;

namespace NEvo.Sagas.Stateful.Building;

public class SagaStateBuilder<TSaga, TState> : IInternalSagaStateBuilder<TSaga> where TSaga : ISaga
{
    private string name;
    Dictionary<Type, IInternalSagaTransitionBuilder<TSaga>> _transitionBuilders = new Dictionary<Type, IInternalSagaTransitionBuilder<TSaga>>();

    public SagaStateBuilder(string name)
    {
        this.name = name;
    }

    public bool IsInitState { get; private set; }

    public SagaStateBuilder<TSaga, TState> InitState() { IsInitState = true; return this; }

    public (IState<TSaga> State, IEnumerable<IInternalSagaTransitionBuilder<TSaga>> TransitionBuilders) Build()
    {
        return (new SagaState<TSaga>(name), _transitionBuilders.Values);
    }

    public SagaTransitionBuilder<TSaga, TMessage> OnMessage<TMessage>(string? name = null) where TMessage : IMessage
    {
        var messageType = typeof(TMessage);
        var transitionBuilder = new SagaTransitionBuilder<TSaga, TMessage>(name ?? messageType.Name);
        _transitionBuilders.Add(messageType, transitionBuilder);
        return transitionBuilder;
    }

    internal void SetActivity(object activity)
    {
        throw new NotImplementedException();
    }
}

using NEvo.Core.StateManaging;

namespace NEvo.Sagas.Stateful;

public interface ISagaMessageTransition<TSaga> : ITransition<TSaga>
{
    public Type MessageType { get; }
}

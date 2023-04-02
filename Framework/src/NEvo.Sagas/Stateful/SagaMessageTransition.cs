using NEvo.Core;
using NEvo.Core.StateManaging;
using NEvo.CQRS.Messaging;

namespace NEvo.Sagas.Stateful;

public class SagaMessageTransition<TSaga, TMessage> : StaticTransition<TSaga, TMessage>, ISagaMessageTransition<TSaga>
                                                                where TMessage : IMessage
                                                                where TSaga : ISaga
{
    public IState<TSaga> DestinationState { get; init; }

    public Type MessageType => typeof(TMessage);

    public SagaMessageTransition(string name, IState<TSaga> destinationState) : base(name)
    {
        DestinationState = Check.Null(destinationState);
    }

    protected override IState<TSaga> GetDestinationState() => DestinationState;
}

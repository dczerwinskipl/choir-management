using NEvo.Core;
using NEvo.Core.StateManaging;
using NEvo.CQRS.Messaging;
using NEvo.Monads;

namespace NEvo.Sagas.Stateful.Building;

public class SagaTransitionBuilder<TSaga, TMessage> : ISagaTransitionBuilder<TSaga>, IInternalSagaTransitionBuilder<TSaga>
                                                                                                                where TSaga : ISaga
                                                                                                                where TMessage : IMessage
{
    private string _name;
    private Maybe<string> _destinationState = Maybe.None;

    public SagaTransitionBuilder(string name)
    {
        _name = name;
    }

    public Maybe<ITransition<TSaga>> Build(Dictionary<string, IState<TSaga>> states)
    {
        return _destinationState.Map<ITransition<TSaga>>(state =>
            new SagaMessageTransition<TSaga, TMessage>(_name, Check.Null(states[state]))
        );
    }

    public void TransitionTo(string stateName)
    {
        _destinationState = stateName;
    }
}

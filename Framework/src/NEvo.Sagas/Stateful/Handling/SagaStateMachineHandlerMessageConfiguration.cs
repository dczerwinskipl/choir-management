using NEvo.CQRS.Messaging;
using NEvo.Sagas.Stateful.Building;

namespace NEvo.Sagas.Stateful.Handling;

public class SagaStateMachineHandlerMessageConfiguration<TSaga, TMessage> : SagaStateMachineHandlerTransitionConfiguration<TSaga> where TSaga : ISaga where TMessage : IMessage
{
    protected override ISagaTransitionBuilder<TSaga> AddTransition<TState>(SagaStateBuilder<TSaga, TState> sagaStateBuilder) => sagaStateBuilder.OnMessage<TMessage>();
}

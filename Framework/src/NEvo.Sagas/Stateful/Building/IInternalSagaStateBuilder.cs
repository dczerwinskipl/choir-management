using NEvo.Core.StateManaging;

namespace NEvo.Sagas.Stateful.Building;

public interface IInternalSagaStateBuilder<TSaga>
{
    bool IsInitState { get; }

    (IState<TSaga> State, IEnumerable<IInternalSagaTransitionBuilder<TSaga>> TransitionBuilders) Build();
}

using NEvo.Core.StateManaging;
using NEvo.Monads;

namespace NEvo.Sagas.Stateful.Building;

public interface IInternalSagaTransitionBuilder<TSaga>
{
    Maybe<ITransition<TSaga>> Build(Dictionary<string, IState<TSaga>> states);
}

namespace NEvo.Sagas.Stateful;

public interface ISagaTransitionBuilder<TSaga>
{
    void TransitionTo(string stateName);
}

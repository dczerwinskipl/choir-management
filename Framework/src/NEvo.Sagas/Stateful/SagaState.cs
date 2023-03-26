using NEvo.Core.StateManaging;
using NEvo.Monads;

namespace NEvo.Sagas.Stateful;

public record SagaState<TSaga>(string Name) : IState<TSaga>
{
    public async Task<Either<Exception, TSaga>> OnEnterAsync(TSaga context)
    {
        //throw new NotImplementedException();
        return context;
    }

    public async Task<Either<Exception, TSaga>> OnExitAsync(TSaga context)
    {
        //throw new NotImplementedException();
        return context;
    }
}

using NEvo.Monads;

namespace NEvo.Core.StateManaging;

public abstract class DynamicTransition<TContext, TTransitionData> : ITransition<TContext, TTransitionData>
{
    public TTransitionData Data { get; init; }

    public string Name { get; init; }

    protected DynamicTransition(TTransitionData data, string name)
    {
        Data = Check.Null(data);
        Name = Check.NullOrEmpty(name);
    }

    public virtual async Task<Either<Refused, IState<TContext>>> ApplyTransitionAsync(TContext context, TTransitionData data)
        => await ValidateTransitionAsync(context, data)
                    .MapAsync(
                        async success => Either.Right<Refused, IState<TContext>>(await GetNextStateAsync()),
                        Either.TaskLeft<Refused, IState<TContext>>
                    );

    protected abstract Task<IState<TContext>> GetNextStateAsync();
    protected async virtual Task<Either<Refused.TransactionValidationFailure, Unit>> ValidateTransitionAsync(TContext context, TTransitionData data) => await Unit.Task;
}

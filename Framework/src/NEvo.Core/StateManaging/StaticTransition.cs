using NEvo.Monads;

namespace NEvo.Core.StateManaging;

public abstract class StaticTransition<TContext, TTransitionData> : ITransition<TContext, TTransitionData>
{
    public string Name { get; init; }

    protected StaticTransition(string name)
    {
        Name = Check.NullOrEmpty(name);
    }

    public virtual async Task<Either<Refused, IState<TContext>>> ApplyTransitionAsync(TContext context, TTransitionData data)
        => await ValidateTransitionAsync(context, data)
                    .BindBi(success => GetDestinationState(), transactionFailure => transactionFailure as Refused);

    protected abstract IState<TContext> GetDestinationState();

    protected virtual async Task<Either<Refused.TransactionValidationFailure, Unit>> ValidateTransitionAsync(TContext context, TTransitionData data) => await Unit.Task;
}

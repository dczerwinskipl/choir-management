using NEvo.Core;

namespace NEvo.Messaging.Queries;

public interface IQueryDispatcher
{
    TResult Dispatch<TResult>(Query<TResult> query) =>
        DispatchAsync(query).ConfigureAwait(false).GetAwaiter().GetResult()
        .Handle(
            result => result,
            failure => throw new AggregateException(failure)
        );

    Task<Try<TResult>> DispatchAsync<TResult>(Query<TResult> query);
}

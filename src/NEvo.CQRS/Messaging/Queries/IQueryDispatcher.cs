using NEvo.Core;

namespace NEvo.Messaging.Queries;

public interface IQueryDispatcher
{
    TResult Dispatch<TResult>(Query<TResult> query) =>
        DispatchAsync(query).ConfigureAwait(false).GetAwaiter().GetResult()
        .Map(
            result => result,
            failure => throw failure
        );

    Task<Either<Exception, TResult>> DispatchAsync<TResult>(Query<TResult> query);
}

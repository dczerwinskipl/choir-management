using NEvo.Monads;

namespace NEvo.CQRS.Messaging.Queries;

public interface IQueryDispatcher
{
    TResult Dispatch<TQuery, TResult>(TQuery query) where TQuery : Query<TResult> =>
        DispatchAsync<TQuery, TResult>(query).ConfigureAwait(false).GetAwaiter().GetResult()
        .Map(
            result => result,
            failure => throw failure
        );

    Task<Either<Exception, TResult>> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : Query<TResult>;
}

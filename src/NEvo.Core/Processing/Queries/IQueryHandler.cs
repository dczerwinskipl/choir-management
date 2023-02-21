using NEvo.Messaging.Queries;

namespace NEvo.Processing.Queries;

public interface IQueryHandler<TQuery, TResult> where TQuery : Query<TResult>
{
    Task<TResult> HandleAsync(TQuery query);
}

public interface IQueryStreamHandler<TQuery, TResult> where TQuery : Query<TResult>
{
    IAsyncEnumerable<TResult> HandleAsync(TQuery query);
}
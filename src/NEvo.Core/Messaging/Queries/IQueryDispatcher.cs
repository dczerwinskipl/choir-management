using NEvo.Messaging.Commands;

namespace NEvo.Messaging.Queries;

public interface IQueryDispatcher
{
    TResult Dispatch<TResult>(Query<TResult> query) => DispatchAsync(query).ConfigureAwait(false).GetAwaiter().GetResult();
    Task<TResult> DispatchAsync<TResult>(Query<TResult> query);
}

using NEvo.Core;

namespace NEvo.Monads;

public static class EitherAsyncExtensions
{
    public static async Task<Either<Exception, Unit>> ToUnit<TRight>(this Task<Either<Exception, TRight>> either) => (await either).ToUnit();

    public static async Task<TResult> Map<TResult, TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Func<TRight, TResult> onSucces, Func<TLeft, TResult> onFailure) => (await either).Map(onSucces, onFailure);
    public static async Task<TResult> MapAsync<TResult, TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Func<TRight, Task<TResult>> onSucces, Func<TLeft, Task<TResult>> onFailure) => await (await either).Map(onSucces, onFailure);

    public static async Task<Either<TLeft, TRight>> Handle<TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Func<TLeft, Either<TLeft, TRight>> onFailure) => (await either).Map(Either.Right<TLeft, TRight>, onFailure);
    public static async Task<TRight> Handle<TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Func<TLeft, TRight> onFailure) => (await either).Map(success => success, onFailure);

    public static async Task<Either<TLeft, Unit>> Handle<TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Func<TLeft, Either<TLeft, Unit>> onFailure) => (await either).Map(success => Either.Right<TLeft, Unit>(Unit.Value), onFailure);
    public static async Task<Unit> Handle<TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Func<TLeft, Unit> onFailure) => (await either).Map(success => Unit.Value, onFailure);


    public static async Task<Either<TLeft, TRight>> Then<TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Action<TRight> action, Func<Exception, TLeft> errorHandler) => (await either).Then(action, errorHandler);
    public static async Task<Either<Exception, TRight>> Then<TRight>(this Task<Either<Exception, TRight>> either, Action<TRight> action) => (await either).Then(action);
    public static async Task<Either<Exception, Unit>> Then(this Task<Either<Exception, Unit>> either, Action action) => (await either).Then(action);


    public static async Task<Either<TLeft, TRight>> ThenAsync<TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Func<TRight, Task> action, Func<Exception, TLeft> errorHandler) => await (await either).ThenAsync(action, errorHandler);
    public static async Task<Either<Exception, TRight>> ThenAsync<TRight>(this Task<Either<Exception, TRight>> either, Func<TRight, Task> action) => await (await either).ThenAsync(action);
    public static async Task<Either<Exception, Unit>> ThenAsync(this Task<Either<Exception, Unit>> either, Func<Task> action) => await (await either).ThenAsync(action);
}
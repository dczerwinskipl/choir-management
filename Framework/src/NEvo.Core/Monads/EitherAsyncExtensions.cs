using NEvo.Core;

namespace NEvo.Monads;

public static class EitherAsyncExtensions
{
    public static async Task<Either<Exception, Unit>> ToUnit<TRight>(this Task<Either<Exception, TRight>> either) => (await either).ToUnit();

    public static async Task<TNewRight> Map<TLeft, TRight, TNewRight>(this Task<Either<TLeft, TRight>> either, Func<TRight, TNewRight> onSucces, Func<TLeft, TNewRight> onFailure) => (await either).Map(onSucces, onFailure);
    public static async Task<Either<TNewLeft, TRight>> Map<TLeft, TRight, TNewLeft>(this Task<Either<TLeft, TRight>> either, Func<TLeft, TNewLeft> onFailure) => (await either).Map(Either.Right<TNewLeft, TRight>, (exc) => Either.Left<TNewLeft, TRight>(onFailure(exc)));
    public static async Task<TNewRight> MapAsync<TLeft, TRight, TNewRight>(this Task<Either<TLeft, TRight>> either, Func<TRight, Task<TNewRight>> onSucces, Func<TLeft, Task<TNewRight>> onFailure) => await (await either).Map(onSucces, onFailure);

    public static async Task<Either<TLeft, TNewRight>> Bind<TLeft, TRight, TNewRight>(this Task<Either<TLeft, TRight>> either, Func<TRight, TNewRight> onSuccess) => (await either).Bind(onSuccess);
    public static async Task<Either<TLeft, TNewRight>> BindAsync<TLeft, TRight, TNewRight>(this Task<Either<TLeft, TRight>> either, Func<TRight, Task<TNewRight>> onSuccess) => await (await either).BindAsync(onSuccess);
    public static async Task<Either<TLeft, TNewRight>> BindAsync<TLeft, TRight, TNewRight>(this Task<Either<TLeft, TRight>> either, Func<TRight, Task<Either<TLeft, TNewRight>>> onSuccess) => await (await either).BindAsync(onSuccess);

    public static async Task<Either<TNewLeft, TRight>> BindLeft<TLeft, TRight, TNewLeft>(this Task<Either<TLeft, TRight>> either, Func<TLeft, TNewLeft> onFailure) => (await either).BindLeft(onFailure);
    public static async Task<Either<TNewLeft, TRight>> BindLeftAsync<TLeft, TRight, TNewLeft>(this Task<Either<TLeft, TRight>> either, Func<TLeft, Task<TNewLeft>> onFailure) => await (await either).BindLeftAsync(onFailure);

    public static async Task<Either<TNewLeft, TNewRight>> BindBi<TLeft, TRight, TNewLeft, TNewRight>(this Task<Either<TLeft, TRight>> either, Func<TRight, TNewRight> onSuccess, Func<TLeft, TNewLeft> onFailure) => (await either).BindBi(onSuccess, onFailure);
    public static async Task<Either<TNewLeft, TNewRight>> BindBiAsync<TLeft, TRight, TNewLeft, TNewRight>(this Task<Either<TLeft, TRight>> either, Func<TRight, Task<TNewRight>> onSuccess, Func<TLeft, TNewLeft> onFailure) => await (await either).BindBiAsync(onSuccess, onFailure);
    public static async Task<Either<TNewLeft, TNewRight>> BindBiAsync<TLeft, TRight, TNewLeft, TNewRight>(this Task<Either<TLeft, TRight>> either, Func<TRight, TNewRight> onSuccess, Func<TLeft, Task<TNewLeft>> onFailure) => await (await either).BindBiAsync(onSuccess, onFailure);
    public static async Task<Either<TNewLeft, TNewRight>> BindBiAsync<TLeft, TRight, TNewLeft, TNewRight>(this Task<Either<TLeft, TRight>> either, Func<TRight, Task<TNewRight>> onSuccess, Func<TLeft, Task<TNewLeft>> onFailure) => await (await either).BindBiAsync(onSuccess, onFailure);


    public static async Task<TRight> Handle<TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Func<TLeft, TRight> onFailure) => (await either).Map(success => success, onFailure);
    public static async Task<Unit> Handle<TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Func<TLeft, Unit> onFailure) => (await either).Map(success => Unit.Value, onFailure);
    public static async Task<Either<TLeft, Unit>> Handle<TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Func<TLeft, Either<TLeft, Unit>> onFailure) => (await either).Map(success => Either.Right<TLeft, Unit>(Unit.Value), onFailure);
    public static async Task<Either<TLeft, TRight>> Handle<TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Func<TLeft, Either<TLeft, TRight>> onFailure) => (await either).Map(Either.Right<TLeft, TRight>, onFailure);
    public static async Task<Either<TNewLeft, TRight>> Handle<TLeft, TRight, TNewLeft>(this Task<Either<TLeft, TRight>> either, Func<TLeft, Either<TNewLeft, TRight>> onFailure) => (await either).Map(Either.Right<TNewLeft, TRight>, onFailure);


    public static async Task<Either<TLeft, TRight>> Then<TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Action<TRight> action, Func<Exception, TLeft> errorHandler) => (await either).Then(action, errorHandler);
    public static async Task<Either<TLeft, TRight>> Then<TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Action<TRight> onSuccess, Action<TLeft> onFailure) => (await either).Then(onSuccess, onFailure);

    public static async Task<Either<Exception, TRight>> Then<TRight>(this Task<Either<Exception, TRight>> either, Action<TRight> action) => (await either).Then(action);

    public static async Task<Either<Exception, Unit>> Then(this Task<Either<Exception, Unit>> either, Action action) => (await either).Then(action);


    public static async Task<Either<TLeft, TRight>> ThenAsync<TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Func<TRight, Task> action, Func<Exception, TLeft> errorHandler) => await (await either).ThenAsync(action, errorHandler);
    public static async Task<Either<TLeft, TRight>> ThenAsync<TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Func<TRight, Task<Either<Exception, Unit>>> action, Func<Exception, TLeft> errorHandler) => await (await either).ThenAsync(action, errorHandler);
    public static async Task<Either<Exception, TRight>> ThenAsync<TRight>(this Task<Either<Exception, TRight>> either, Func<TRight, Task> action) => await (await either).ThenAsync(action);
    public static async Task<Either<Exception, Unit>> ThenAsync(this Task<Either<Exception, Unit>> either, Func<Task> action) => await (await either).ThenAsync(action);


    public static async Task<Either<TLeft, TRight>> OnFailure<TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Action<TLeft> action)
        => (await either).OnFailure(action);
    public static async Task<Either<TLeft, TRight>> OnFailureAsync<TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Func<TLeft, Task> action)
        => await (await either).OnFailureAsync(action);
}
using Newtonsoft.Json.Linq;

namespace NEvo.Monads;

public static class MaybeAsyncExtensions
{
    public static async Task<Maybe<TResult>> Map<T, TResult>(this Task<Maybe<T>> maybe, Func<T, TResult> convert) => (await maybe).Map(convert);
    public static async Task<Maybe<TResult>> MapAsync<T, TResult>(this Task<Maybe<T>> maybe, Func<T, Task<TResult>> convert) => await (await maybe).MapAsync(convert);

    public static async Task<Maybe<TResult>> Bind<T, TResult>(this Task<Maybe<T>> maybe, Func<T, Maybe<TResult>> convert) => (await maybe).Bind(convert);
    public static async Task<Maybe<TResult>> BindAsync<T, TResult>(this Task<Maybe<T>> maybe, Func<T, Task<Maybe<TResult>>> convert) => await (await maybe).BindAsync(convert);

    public static async Task<TResult> Match<T, TResult>(this Task<Maybe<T>> maybe, Func<T, TResult> some, Func<TResult> none) => (await maybe).Match(some, none);
    public static async Task<TResult> MatchAsync<T, TResult>(this Task<Maybe<T>> maybe, Func<T, Task<TResult>> some, Func<Task<TResult>> none) => await (await maybe).MatchAsync(some, none);


    public static async Task<Either<Exception, TResult>> Match<T, TResult>(this Task<Maybe<T>> maybe, Func<T, Either<Exception, TResult>> some, Func<Exception> none) => (await maybe).Match(some, none);
    public static async Task<Either<Exception, TResult>> MatchAsync<T, TResult>(this Task<Maybe<T>> maybe, Func<T, Task<Either<Exception, TResult>>> some, Func<Exception> none) => await (await maybe).MatchAsync(some, none);
}
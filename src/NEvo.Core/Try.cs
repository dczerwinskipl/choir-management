using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;

namespace NEvo.Core;

/// <summary>
/// Helper to create Try class instnace
/// </summary>
public static class Try
{
    public static Either<Exception, Unit> ToUnit<TRight>(this Either<Exception, TRight> either) => either.Map(_ => Either.Right(), Either.Left);

    public static Either<Exception, Unit> Of(Action action)
    {
        try
        {
            action();
            return Either.Right();
        }
        catch (Exception exc)
        {
            return Either.Left(exc);
        }
    }

    public static Either<Exception, TResult> Of<TResult>(Func<Either<Exception, TResult>> action)
    {
        try
        {
            return action();
        }
        catch (Exception exc)
        {
            return Either.Left<TResult>(exc);
        }
    }

    public static Either<Exception, TResult> Of<TResult>(Func<TResult> action)
    {
        try
        {
            return Either.Right(action());
        }
        catch (Exception exc)
        {
            return Either.Left<TResult>(exc);
        }
    }

    public static async Task<Either<Exception, Unit>> OfAsync(Func<Task> action)
    {
        try
        {
            await action();
            return Either.Right();
        }
        catch (Exception exc)
        {
            return Either.Left(exc);
        }
    }
    public static async Task<Either<Exception, TResult>> OfAsync<TResult>(Func<Task<Either<Exception, TResult>>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception exc)
        {
            return Either.Left<TResult>(exc);
        }
    }

    public static async Task<Either<Exception, TRight>> OfAsync<TLeft, TRight>(Func<Task<Either<TLeft, TRight>>> action, Func<TLeft, Exception> errorHandler)
    {
        try
        {
            return await action().Map(Either.Right<Exception, TRight>, error => Either.Left<TRight>(errorHandler(error)));
        }
        catch (Exception exc)
        {
            return Either.Left<Exception, TRight>(exc);
        }
    }

    public static async Task<Either<Exception, TResult>> OfAsync<TResult>(Func<Task<TResult>> action)
    {
        try
        {
            return Either.Right(await action());
        }
        catch (Exception exc)
        {
            return Either.Left<TResult>(exc);
        }
    }

    public static Either<TLeft, TRight> Then<TLeft, TRight>(this Either<TLeft, TRight> either, Action<TRight> action, Func<Exception, TLeft> errorHandler)
    {
        if(either.IsRight)
        {
            return Of(() => action(either.Right)).Map(s => Either.Right<TLeft, TRight>(either.Right), exc => Either.Left<TLeft, TRight>(errorHandler(exc)));
        }

        return either;
    }

    public static Either<Exception, TRight> Then<TRight>(this Either<Exception, TRight> either, Action<TRight> action)
    {
        if (either.IsRight)
        {
            return Of(() => action(either.Right)).Map(s => Either.Right(either.Right), Either.Left<TRight>);
        }

        return either;
    }

    public static Either<Exception, Unit> Then(this Either<Exception, Unit> either, Action action)
    {
        if (either.IsRight)
        {
            return Of(action);
        }

        return either;
    }


    public static async Task<Either<TLeft, TRight>> ThenAsync<TLeft, TRight>(this Either<TLeft, TRight> either, Func<TRight, Task> action, Func<Exception, TLeft> errorHandler)
    {
        if (either.IsRight)
        {
            return await OfAsync(() => action(either.Right)).Map(s => Either.Right<TLeft, TRight>(either.Right), exc => Either.Left<TLeft, TRight>(errorHandler(exc)));
        }

        return either;
    }

    public static async Task<Either<Exception, TRight>> ThenAsync<TRight>(this Either<Exception, TRight> either, Func<TRight, Task> action)
    {
        if (either.IsRight)
        {
            return await OfAsync(() => action(either.Right)).Map(s => Either.Right(either.Right), Either.Left<TRight>);
        }

        return either;
    }

    public static async Task<Either<Exception, Unit>> ThenAsync(this Either<Exception, Unit> either, Func<Task> action)
    {
        if (either.IsRight)
        {
            return await OfAsync(action);
        }

        return either;
    }

    /*
    public static async Task<Either<Exception, TOther>> ThenAsync<TResult, TOther>(this Task<Either<Exception, TResult>> @Either, Func<TResult, Task<TOther>> onSuccess) =>
        await (await @Either).ThenAsync(result => onSuccess(result));

    public static async Task<Either<Exception, Unit>> ThenAsync<TResult>(this Task<Either<Exception, TResult>> @Either, Func<TResult, Task> onSuccess) =>
    await (await @Either).ThenAsync(async result => { await onSuccess(result); return Success(); });

    public static async Task<Either<Exception, TOther>> ThenAsync<TResult, TOther>(this Task<Either<Exception, TResult>> @Either, Func<TResult, TOther> onSuccess) =>
        await (await @Either).ThenAsync(result => Task.FromResult(onSuccess(result)));
    public static async Task<Either<Exception, TOther>> ThenAsync<TResult, TOther>(this Task<Either<Exception, TResult>> @Either, Func<TResult, Either<Exception, TOther>> onSuccess) =>
        await (await @Either).ThenAsync(result => Task.FromResult(onSuccess(result)));


    public static async Task<Either<Exception, TOther>> ThenAsync<TResult, TOther>(this Task<Either<Exception, TResult>> @Either, Func<TResult, Task<Either<Exception, TOther>>> onSuccess) =>
       await (await @Either).ThenAsync(onSuccess);

    public static async Task<Either<Exception, TResult>> Catch<TResult>(this Task<Either<Exception, TResult>> @Either, Func<Exception, Either<Exception, TResult>> onFailure) =>
        await (await @Either).HandleAsync(success => Success(success), onFailure);

    public static async Task<Either<Exception, TOther>> HandleAsync<TResult, TOther>(this Task<Either<Exception, TResult>> @Either, Func<TResult, Either<Exception, TOther>> onSuccess, Func<Exception, Either<Exception, TOther>> onFailure) =>
        await (await @Either).HandleAsync(onSuccess, onFailure);

    public static async Task<Either<Exception, TOther>> HandleAsync<TResult, TOther>(this Task<Either<Exception, TResult>> @Either, Func<TResult, Task<Either<Exception, TOther>>> onSuccess, Func<Exception, Task<Either<Exception, TOther>>> onFailure) =>
        await (await @Either).HandleAsync(onSuccess, onFailure);*/
}

public static class EitherAsyncExtensions
{
    public static async Task<Either<Exception, Unit>> ToUnit<TRight>(this Task<Either<Exception, TRight>> either) => (await either).ToUnit();

    public static async Task<TResult> Map<TResult, TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Func<TRight, TResult> onSucces, Func<TLeft, TResult> onFailure) => (await either).Map(onSucces, onFailure);
    public static async Task<TResult> MapAsync<TResult, TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Func<TRight, Task<TResult>> onSucces, Func<TLeft, Task<TResult>> onFailure) => await (await either).Map(onSucces, onFailure);
    
    public static async Task<Either<TLeft, TRight>> Handle<TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Func<TLeft, Either<TLeft, TRight>> onFailure) => (await either).Map(Either.Right<TLeft, TRight>, onFailure);
    public static async Task<TRight> Handle<TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Func<TLeft, TRight> onFailure) => (await either).Map(success => success, onFailure);

    public static async Task<Either<TLeft, TRight>> Then<TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Action<TRight> action, Func<Exception, TLeft> errorHandler) => (await either).Then(action, errorHandler);
    public static async Task<Either<Exception, TRight>> Then<TRight>(this Task<Either<Exception, TRight>> either, Action<TRight> action) => (await either).Then(action);
    public static async Task<Either<Exception, Unit>> Then(this Task<Either<Exception, Unit>> either, Action action) => (await either).Then(action);


    public static async Task<Either<TLeft, TRight>> ThenAsync<TLeft, TRight>(this Task<Either<TLeft, TRight>> either, Func<TRight, Task> action, Func<Exception, TLeft> errorHandler) => await (await either).ThenAsync(action, errorHandler);
    public static async Task<Either<Exception, TRight>> ThenAsync<TRight>(this Task<Either<Exception, TRight>> either, Func<TRight, Task> action) => await (await either).ThenAsync(action);
    public static async Task<Either<Exception, Unit>> ThenAsync(this Task<Either<Exception, Unit>> either, Func<Task> action) => await (await either).ThenAsync(action);
}
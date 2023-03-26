using NEvo.Core;

namespace NEvo.Monads;

/// <summary>
/// Helper to create Try class instnace
/// </summary>
public static class Try
{
    public static Either<Exception, Unit> Of(Action action)
    {
        try
        {
            action();
            return Either.Success();
        }
        catch (Exception exc)
        {
            return Either.Failure(exc);
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
            return Either.Failure<TResult>(exc);
        }
    }

    public static Either<Exception, TResult> Of<TResult>(Func<TResult> action)
    {
        try
        {
            return Either.Success(action());
        }
        catch (Exception exc)
        {
            return Either.Failure<TResult>(exc);
        }
    }


    public static async Task<Either<Exception, Unit>> OfAsync(Func<Task> action)
    {
        try
        {
            await action();
            return Either.Success();
        }
        catch (Exception exc)
        {
            return Either.Failure(exc);
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
            return Either.Failure<TResult>(exc);
        }
    }

    public static async Task<Either<Exception, TRight>> OfAsync<TLeft, TRight>(Func<Task<Either<TLeft, TRight>>> action, Func<TLeft, Exception> errorHandler)
    {
        try
        {
            return await action().Map(Either.Right<Exception, TRight>, error => Either.Failure<TRight>(errorHandler(error)));
        }
        catch (Exception exc)
        {
            return Either.Left<Exception, TRight>(exc);
        }
    }

    public static async Task<Either<Exception, TRight>> OfAsync<TLeft, TRight>(Func<Task<Either<TLeft, TRight>>> action, Func<TLeft, Task<Either<Exception, TRight>>> errorHandler)
    {
        try
        {
            return await action().MapAsync(Either.TaskRight<Exception, TRight>, async error => await errorHandler(error));
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
            return Either.Success(await action());
        }
        catch (Exception exc)
        {
            return Either.Failure<TResult>(exc);
        }
    }


    public static Either<TLeft, TRight> Then<TLeft, TRight>(this Either<TLeft, TRight> either, Action<TRight> action, Func<Exception, TLeft> errorHandler)
    {
        if (either.IsRight)
        {
            return Of(() => action(either.Right)).Map(s => Either.Right<TLeft, TRight>(either.Right), exc => Either.Left<TLeft, TRight>(errorHandler(exc)));
        }

        return either;
    }

    public static Either<Exception, TRight> Then<TRight>(this Either<Exception, TRight> either, Action<TRight> action)
    {
        if (either.IsRight)
        {
            return Of(() => action(either.Right)).Map(s => Either.Success(either.Right), Either.Failure<TRight>);
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

    public static async Task<Either<TLeft, TRight>> ThenAsync<TLeft, TRight>(this Either<TLeft, TRight> either, Func<TRight, Task<Either<Exception, Unit>>> action, Func<Exception, TLeft> errorHandler)
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
            return await OfAsync(() => action(either.Right)).Map(s => Either.Success(either.Right), Either.Failure<TRight>);
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
}

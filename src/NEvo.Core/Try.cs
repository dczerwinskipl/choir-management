using System.Collections.Generic;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;

namespace NEvo.Core;

public struct Try<TResult>
{
    private readonly Exception? _exception;
    private readonly TResult? _result;

    /// <summary>
    /// Returns true if result is success
    /// </summary>
    public readonly bool IsFailure;

    /// <summary>
    /// Returns true if result is failure
    /// </summary>
    public bool IsSuccess => !IsFailure;

    internal Try(Exception exc)
    {
        _exception = exc;
        _result = default;
        IsFailure = true;
    }

    internal Try(TResult result)
    {
        _result = result;
        _exception = default;
        IsFailure = false;
    }

    /// <summary>
    /// Returns value if result is failure
    /// </summary>
    public Exception Exception
    {
        get
        {
            if (!IsFailure)
                throw new InvalidOperationException("Not in the left state");
#pragma warning disable CS8603 // Possible null reference return.
            return _exception;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }

    /// <summary>
    /// Returns value if result is success
    /// </summary>
    public TResult Result
    {
        get
        {
            if (!IsSuccess)
                throw new InvalidOperationException("Not in the right state");
#pragma warning disable CS8603 // Possible null reference return.
            return _result;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }

    /// <summary>
    /// Map success or failure to new output
    /// </summary>
    public TOther Handle<TOther>(Func<TResult, TOther> onSucces, Func<Exception, TOther> onFailure)
        => IsSuccess ? onSucces(Result) : onFailure(Exception);

    public Task<TOther> HandleAsync<TOther>(Func<TResult, Task<TOther>> onSucces, Func<Exception, Task<TOther>> onFailure)
        => IsSuccess ? onSucces(Result) : onFailure(Exception);

    public Try<TOther> Cast<TOther>()  => Handle(success => Try.Success((TOther)(object)success), Try.Failure<TOther>);

    /// <summary>
    /// Execute action when fail
    /// </summary>
    public Try<TResult> OnFailure(Action<Exception> onFailure)
    {
        if (IsFailure)
        {
            onFailure(Exception);
        }
        return this;
    }

    /// <summary>
    /// Execute action when success
    /// </summary>
    public Try<TResult> OnSuccess(Action<TResult> onSuccess)
    {
        if (IsSuccess)
        {
            onSuccess(Result);
        }
        return this;
    }

    public Task<Try<TOther>> ThenAsync<TOther>(Func<Task<Try<TOther>>> onSuccess) => HandleAsync(_ => Try.OfAsync(onSuccess), Try.TaskFailure<TOther>);
    public Task<Try<TOther>> ThenAsync<TOther>(Func<Task<TOther>> onSuccess) => HandleAsync(_ => Try.OfAsync(onSuccess), Try.TaskFailure<TOther>);

    public static implicit operator Either<Exception, TResult>(Try<TResult> @try) => @try.Handle(success => new Either<Exception, TResult>(success), exception => new Either<Exception, TResult>(exception));
    public static explicit operator Try<TResult>(Either<Exception, TResult> either) => either.Handle(success => new Try<TResult>(success), exception => new Try<TResult>(exception));
}


/// <summary>
/// Helper to create Try class instnace
/// </summary>
public static class Try
{
    /// <summary>
    /// Create reponse for failure execution
    /// </summary>
    /// <typeparam name="TRight">Type returned when success</typeparam>
    /// <param name="exception">Exception</param>
    /// <returns>New Try instance that represents failure</returns>
    public static Try<TRight> Failure<TRight>(Exception exception) => new(exception);

    /// <summary>
    /// Create response for success execution
    /// </summary>
    /// <typeparam name="TRight">Type returned when success</typeparam>
    /// <param name="right">Result of success execution</param>
    /// <returns>New Try instance that represents success</returns>
    public static Try<TRight> Success<TRight>(TRight right) => new(right);


    public static Try<Unit> Failure(Exception exc) => new(exc);
    public static Try<Unit> Success() => new(Unit.Value);

    public static Task<Try<TResult>> TaskFailure<TResult>(Exception exc) => Task.FromResult(Failure<TResult>(exc));
    public static Task<Try<TResult>> TaskSuccess<TResult>(TResult result) => Task.FromResult(Success(result));
    public static Task<Try<Unit>> TaskFailure(Exception exc) => Task.FromResult(Failure(exc));
    public static Task<Try<Unit>> TaskSuccess() => Task.FromResult(Success());

    public static Try<Unit> Of(Action action)
    {
        try
        {
            action();
            return Success();
        } 
        catch(Exception exc)
        {
            return Failure(exc);
        }
    }

    public static Try<TResult> Of<TResult>(Func<TResult> action)
    {
        try
        {
            return Success(action());
        }
        catch (Exception exc)
        {
            return Failure<TResult>(exc);
        }
    }

    public static Try<TRight> Of<TRight>(Func<Either<Exception, TRight>> action)
    {
        try
        {
            return (Try<TRight>)action();
        }
        catch (Exception exc)
        {
            return Failure<TRight>(exc);
        }
    }

    public static async Task<Try<Unit>> OfAsync(Func<Task> action)
    {
        try
        {
            await action();
            return Success();
        }
        catch (Exception exc)
        {
            return Failure(exc);
        }
    }
    public static async Task<Try<TResult>> OfAsync<TResult>(Func<Task<Try<TResult>>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception exc)
        {
            return Failure<TResult>(exc);
        }
    }

    public static async Task<Try<TResult>> OfAsync<TResult>(Func<Task<TResult>> action)
    {
        try
        {
            return Success(await action());
        }
        catch (Exception exc)
        {
            return Failure<TResult>(exc);
        }
    }

    public static async Task<Try<TResult>> OfAsync<TResult>(Func<Task<Either<Exception, TResult>>> action)
    {
        try
        {
            return (Try<TResult>)await action();
        }
        catch (Exception exc)
        {
            return Failure<TResult>(exc);
        }
    }

    public static async Task<Try<TOther>> ThenAsync<TResult, TOther>(this Task<Try<TResult>> @try, Func<Task<Try<TOther>>> onSuccess) =>
       await (await @try).ThenAsync(onSuccess);

    public static async Task<Try<TOther>> ThenAsync<TResult, TOther>(this Task<Try<TResult>> @try, Func<Task<TOther>> onSuccess) =>
        await (await @try).ThenAsync(onSuccess);
}

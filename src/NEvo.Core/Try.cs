using System.Collections.Generic;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;

namespace NEvo.Core;

public struct Try<TRight>
{
    private readonly Exception? _exception;
    private readonly TRight? _result;

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

    internal Try(TRight result)
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
    public TRight Result
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
    public TResult Handle<TResult>(Func<TRight, TResult> onSucces, Func<Exception, TResult> onFailure)
        => IsSuccess ? onSucces(Result) : onFailure(Exception);

    /// <summary>
    /// Execute action when fail
    /// </summary>
    public Try<TRight> OnFailure(Action<Exception> onFailure)
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
    public Try<TRight> OnSuccess(Action<TRight> onSuccess)
    {
        if (IsSuccess)
        {
            onSuccess(Result);
        }
        return this;
    }

    public static implicit operator Either<Exception, TRight>(Try<TRight> @try) => @try.Handle(success => new Either<Exception, TRight>(success), exception => new Either<Exception, TRight>(exception));
    public static explicit operator Try<TRight>(Either<Exception, TRight> either) => either.Handle(success => new Try<TRight>(success), exception => new Try<TRight>(exception));
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
}

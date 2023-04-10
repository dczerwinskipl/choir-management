using NEvo.Core;

namespace NEvo.Monads;

/// <summary>
/// Class that represents alternative results of execution.
/// </summary>
/// <typeparam name="TLeft">Type returned when failed</typeparam>
/// <typeparam name="TRight">Type returned when success</typeparam>
public readonly struct Either<TLeft, TRight>
{
    private readonly TLeft? _left;
    private readonly TRight? _right;

    /// <summary>
    /// Returns true if result is success
    /// </summary>
    public readonly bool IsLeft;

    /// <summary>
    /// Returns true if result is failure
    /// </summary>
    public bool IsRight => !IsLeft;

    internal Either(TLeft left)
    {
        _left = left;
        _right = default;
        IsLeft = true;
    }

    internal Either(TRight right)
    {
        _right = right;
        _left = default;
        IsLeft = false;
    }

    /// <summary>
    /// Returns value if result is failure
    /// </summary>
    public TLeft Left
    {
        get
        {
            if (!IsLeft)
                throw new InvalidOperationException("Not in the left state");
#pragma warning disable CS8603 // Possible null reference return.
            return _left;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }

    /// <summary>
    /// Returns value if result is success
    /// </summary>
    public TRight Right
    {
        get
        {
            if (!IsRight)
                throw new InvalidOperationException("Not in the right state");
#pragma warning disable CS8603 // Possible null reference return.
            return _right;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }

    /// <summary>
    /// Map success or failure to new output
    /// </summary>
    public TNewRight Map<TNewRight>(Func<TRight, TNewRight> onSuccess, Func<TLeft, TNewRight> onFailure)
        => IsRight ? onSuccess(Right) : onFailure(Left);

    /// <summary>
    /// Map success or failure to new output
    /// </summary>
    public async Task<TNewRight> MapAsync<TNewRight>(Func<TRight, Task<TNewRight>> onSuccess, Func<TLeft, Task<TNewRight>> onFailure)
        => await (IsRight ? onSuccess(Right) : onFailure(Left));

    public Either<TLeft, TNewRight> Bind<TNewRight>(Func<TRight, TNewRight> onSuccess)
        => IsRight ? onSuccess(Right) : Either.Left<TLeft, TNewRight>(Left);

    public Either<TLeft, TNewRight> Bind<TNewRight>(Func<TRight, Either<TLeft, TNewRight>> onSuccess)
    => IsRight ? onSuccess(Right) : Either.Left<TLeft, TNewRight>(Left);

    public async Task<Either<TLeft, TNewRight>> BindAsync<TNewRight>(Func<TRight, Task<TNewRight>> onSuccess)
        => IsRight ? await onSuccess(Right) : Either.Left<TLeft, TNewRight>(Left);

    public async Task<Either<TLeft, TNewRight>> BindAsync<TNewRight>(Func<TRight, Task<Either<TLeft, TNewRight>>> onSuccess)
    => IsRight ? await onSuccess(Right) : Either.Left<TLeft, TNewRight>(Left);

    public Either<TNewLeft, TRight> BindLeft<TNewLeft>(Func<TLeft, TNewLeft> onFailure)
        => IsRight ? Right : Either.Left<TNewLeft, TRight>(onFailure(Left));

    public async Task<Either<TNewLeft, TRight>> BindLeftAsync<TNewLeft>(Func<TLeft, Task<TNewLeft>> onFailure)
        => IsRight ? Right : Either.Left<TNewLeft, TRight>(await onFailure(Left));


    public Either<TNewLeft, TNewRight> BindBi<TNewLeft, TNewRight>(Func<TRight, TNewRight> onSuccess, Func<TLeft, TNewLeft> onFailure)
        => IsRight ? Either.Right<TNewLeft, TNewRight>(onSuccess(Right)) : Either.Left<TNewLeft, TNewRight>(onFailure(Left));

    public async Task<Either<TNewLeft, TNewRight>> BindBiAsync<TNewLeft, TNewRight>(Func<TRight, Task<TNewRight>> onSuccess, Func<TLeft, TNewLeft> onFailure)
        => IsRight ? Either.Right<TNewLeft, TNewRight>(await onSuccess(Right)) : Either.Left<TNewLeft, TNewRight>(onFailure(Left));

    public async Task<Either<TNewLeft, TNewRight>> BindBiAsync<TNewLeft, TNewRight>(Func<TRight, TNewRight> onSuccess, Func<TLeft, Task<TNewLeft>> onFailure)
        => IsRight ? Either.Right<TNewLeft, TNewRight>(onSuccess(Right)) : Either.Left<TNewLeft, TNewRight>(await onFailure(Left));

    public async Task<Either<TNewLeft, TNewRight>> BindBiAsync<TNewLeft, TNewRight>(Func<TRight, Task<TNewRight>> onSuccess, Func<TLeft, Task<TNewLeft>> onFailure)
        => IsRight ? Either.Right<TNewLeft, TNewRight>(await onSuccess(Right)) : Either.Left<TNewLeft, TNewRight>(await onFailure(Left));


    /// <summary>
    /// TODO: to mi nie pasuje
    /// </summary>
    public Either<TLeft, TRight> Then(Action<TRight> onSuccess, Action<TLeft> onFailure)
    {
        if (IsRight)
        {
            onSuccess(Right);
        }
        else
        {
            onFailure(Left);
        }

        return this;
    }

    /// <summary>
    /// Execute action when fail
    /// </summary>
    public Either<TLeft, TRight> OnFailure(Action<TLeft> onFailure)
    {
        if (IsLeft)
        {
            onFailure(Left);
        }
        return this;
    }

    public async Task<Either<TLeft, TRight>> OnFailureAsync(Func<TLeft, Task> onFailure)
    {
        if (IsLeft)
        {
            await onFailure(Left);
        }
        return this;
    }

    /// <summary>
    /// Execute action when success
    /// </summary>
    public Either<TLeft, TRight> OnSuccess(Action<TRight> onSuccess)
    {
        if (IsRight)
        {
            onSuccess(Right);
        }
        return this;
    }

    public Either<TLeft, TNewRight> Cast<TNewRight>() => Map(success => Either.Right<TLeft, TNewRight>((TNewRight)(object)success), Either.Left<TLeft, TNewRight>);

    public Either<TLeft, TRight> Handle(Func<TLeft, Either<TLeft, TRight>> onFailure) => Map(Either.Right<TLeft, TRight>, onFailure);

    public TRight Handle(Func<TLeft, TRight> onFailure) => Map(success => success, onFailure);

    public static implicit operator Either<TLeft, Unit>(Either<TLeft, TRight> either) => either.Map(success => Either.Right<TLeft, Unit>(Unit.Value), Either.Left<TLeft, Unit>);
    public static implicit operator Either<TLeft, TRight>(TLeft failure) => Either.Left<TLeft, TRight>(failure);
    public static implicit operator Either<TLeft, TRight>(TRight success) => Either.Right<TLeft, TRight>(success);
}
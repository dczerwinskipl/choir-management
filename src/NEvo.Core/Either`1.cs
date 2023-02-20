using System.ComponentModel;

namespace NEvo.Core;

/// <summary>
/// Class that represents alternative results of execution.
/// </summary>
/// <typeparam name="TLeft">Type returned when failed</typeparam>
/// <typeparam name="TRight">Type returned when success</typeparam>
public struct Either<TLeft, TRight>
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
    public TResult Handle<TResult>(Func<TRight, TResult> onSucces, Func<TLeft, TResult> onFailure)
        => IsRight ? onSucces(Right) : onFailure(Left);

    /// <summary>
    /// Execute action when success or failure
    /// </summary>
    public Either<TLeft, TRight> Handle(Action<TRight> onSucces, Action<TLeft> onFailure)
    { 
        if (IsRight) { 
            onSucces(Right); 
        } 
        else { 
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

    /// <summary>
    /// Execute action when success
    /// </summary>
    public Either<TLeft, TRight> OnSuccess(Action<TLeft> onFailure)
    {
        if (IsRight)
        {
            onFailure(Left);
        }
        return this;
    }
}
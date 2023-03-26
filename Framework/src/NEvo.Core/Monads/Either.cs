using NEvo.Core;

namespace NEvo.Monads;

/// <summary>
/// Helper to create Either class instnace
/// </summary>
public static class Either
{
    /// <summary>
    /// Create reponse for failure execution
    /// </summary>
    /// <typeparam name="TLeft">Type returned when failed</typeparam>
    /// <typeparam name="TRight">Type returned when success</typeparam>
    /// <param name="left">Additional informations about failure, p.ex. Exception</param>
    /// <returns>New Either instance that represents failure</returns>
    public static Either<TLeft, TRight> Left<TLeft, TRight>(TLeft left) => new(left);

    /// <summary>
    /// Create response for success execution
    /// </summary>
    /// <typeparam name="TLeft">Type returned when failed</typeparam>
    /// <typeparam name="TRight">Type returned when success</typeparam>
    /// <param name="right">Result of success execution</param>
    /// <returns>New Either instance that represents success</returns>
    public static Either<TLeft, TRight> Right<TLeft, TRight>(TRight right) => new(right);

    /// <summary>
    /// Simplified version to create reponse for failure execution. Left type is always Exception
    /// </summary>
    /// <typeparam name="TRight">Type returned when success</typeparam>
    /// <param name="exc">Exception</param>
    /// <returns>New Either  instance that represents failure</returns>
    public static Either<Exception, TRight> Failure<TRight>(Exception exc) => new(exc);

    /// <summary>
    /// Simplified version to create reponse for success execution. Left type is always Exception
    /// </summary>
    /// <typeparam name="TRight">Type returned when success</typeparam>
    /// <param name="right">Result of success execution</param>
    /// <returns>New Either  instance that represents success</returns>
    public static Either<Exception, TRight> Success<TRight>(TRight right) => new(right);

    /// <summary>
    /// Simplified version to create reponse for failure execution of void methods. Left type is always Exception
    /// </summary>
    /// <typeparam name="TRight">Type returned when success</typeparam>
    /// <param name="exc">Exception</param>
    /// <returns>New Either  instance that represents failure</returns>
    public static Either<Exception, Unit> Failure(Exception exc) => new(exc);

    /// <summary>
    /// Simplified version to create reponse for success execution of void methods. Left type is always Exception
    /// </summary>
    /// <typeparam name="TRight">Type returned when success</typeparam>
    /// <param name="right">Result of success execution</param>
    /// <returns>New Either  instance that represents success</returns>
    public static Either<Exception, Unit> Success() => new(Unit.Value);

    /// <summary>
    /// Simplified version to create reponse for failure execution of void methods. Left type is always Exception
    /// </summary>
    /// <typeparam name="TRight">Type returned when success</typeparam>
    /// <param name="exc">Exception</param>
    /// <returns>New Either  instance that represents failure</returns>
    public static Task<Either<Exception, Unit>> TaskFailure(Exception exc) => Task.FromResult(new Either<Exception, Unit>(exc));

    public static Task<Either<Exception, TRight>> TaskFailure<TRight>(Exception exc) => Task.FromResult(new Either<Exception, TRight>(exc));

    public static Task<Either<TLeft, TRight>> TaskLeft<TLeft, TRight>(TLeft error) => Task.FromResult(new Either<TLeft, TRight>(error));

    /// <summary>
    /// Simplified version to create reponse for success execution of void methods. Left type is always Exception
    /// </summary>
    /// <typeparam name="TRight">Type returned when success</typeparam>
    /// <param name="right">Result of success execution</param>
    /// <returns>New Either  instance that represents success</returns>
    public static Task<Either<Exception, Unit>> TaskSuccess() => Task.FromResult(new Either<Exception, Unit>(Unit.Value));

    public static Task<Either<Exception, TRight>> TaskSuccess<TRight>(TRight result) => Task.FromResult(new Either<Exception, TRight>(result));

    public static Task<Either<TLeft, TRight>> TaskRight<TLeft, TRight>(TRight result) => Task.FromResult(new Either<TLeft, TRight>(result));

    public static Either<Exception, Unit> ToUnit<TRight>(this Either<Exception, TRight> either) => either.Map(_ => Success(), Failure);
}

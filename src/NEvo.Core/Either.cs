namespace NEvo.Core;

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
    public static Either<Exception, TRight> Left<TRight>(Exception exc) => new(exc);

    /// <summary>
    /// Simplified version to create reponse for success execution. Left type is always Exception
    /// </summary>
    /// <typeparam name="TRight">Type returned when success</typeparam>
    /// <param name="right">Result of success execution</param>
    /// <returns>New Either  instance that represents success</returns>
    public static Either<Exception, TRight> Left<TRight>(TRight right) => new(right);
}
